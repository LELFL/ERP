using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using System.Diagnostics;

namespace ELF.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedDataAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly IServiceProvider serviceProvider;
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        this.serviceProvider = serviceProvider;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var strategy = db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                await db.Database.MigrateAsync();
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedDataAsync()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordEncryptor = scope.ServiceProvider.GetRequiredService<IPasswordEncryptor>();
            await db.Database.EnsureCreatedAsync();

            var role = new Role { Name = "超级管理员", Description = "超级管理员拥有所有权限" };
            // 添加角色种子数据
            if (!await db.Roles.AsNoTracking().AnyAsync(x => x.Name == "超级管理员"))
            {
                db.Roles.Add(role);
                await db.SaveChangesAsync();
            }

            // 添加用户种子数据
            if (await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == "admin") == null)
            {
                var pwd = passwordEncryptor.Encrypt("123456"); ;
                var user = new User { Name = "admin", Nickname = "超级管理员", Password = pwd };
                db.Users.Add(user);
                user.Roles.Add(role);
                await db.SaveChangesAsync();
            }


            // 获取OpenIddict的应用、作用域和授权管理器
            var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

            // 检查并添加作用域
            if (await scopeManager.FindByNameAsync("api") == null)
            {
                await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "api",
                    DisplayName = "Access to API",
                    Resources = { "resource_api_1" },
                });
            }

            // 检查并添加 offline_access 作用域
            if (await scopeManager.FindByNameAsync(OpenIddictConstants.Scopes.OfflineAccess) == null)
            {
                await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = OpenIddictConstants.Scopes.OfflineAccess,
                    DisplayName = "Offline Access",
                    Resources = { "resource_api_1" },
                });
            }

            if (await applicationManager.FindByClientIdAsync("employee_client") is null)
            {
                // 员工后台客户端（机密客户端，Authorization Code + PKCE）
                var adminClient = new OpenIddictApplicationDescriptor
                {
                    ClientId = "employee_client",
                    ClientSecret = "ds1fa2d1g23sg456dsda4g6s63.-45sj", // 使用 PasswordHasher 或其他方式加密
                    DisplayName = "后台管理系统系统",
                    //RedirectUris = { new Uri("https://admin.example.com/signin-oidc") },
                    //PostLogoutRedirectUris = { new Uri("https://admin.example.com/signout-callback") },
                    Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    // 这里添加允许的作用域
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    // 添加offline_access作用域
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess,
                }
                };

                // 客户商城客户端（公共客户端，Implicit/Hybrid Flow）
                var mallClient = new OpenIddictApplicationDescriptor
                {
                    ClientId = "customer_client",
                    ClientSecret = "sd56a4g5gd4fsdj545g.-45sj", // 使用 PasswordHasher 或其他方式加密
                    DisplayName = "客户商城系统",
                    RedirectUris = { new Uri("https://identityservice/callback") },
                    PostLogoutRedirectUris = { new Uri("https://identityservice/signout-callback") },
                    Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    // 这里添加允许的作用域
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    // 添加offline_access作用域
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess,
                },
                    //Type = OpenIddictConstants.ClientTypes.Public
                };

                await applicationManager.CreateAsync(adminClient);
                await applicationManager.CreateAsync(mallClient);

            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
}
