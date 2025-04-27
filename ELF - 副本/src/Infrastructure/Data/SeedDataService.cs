using System.Diagnostics;
using ELF.Infrastructure.Data;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace ELF.Data;
internal class SeedDataService(
    IServiceProvider serviceProvider,
    ILogger<SeedDataService> logger) : ISeedDataService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "迁移或初始化发生错误");
            throw;
        }

        //hostApplicationLifetime.StopApplication();
    }

    private async Task RunMigrationAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    public async Task SeedDataAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordEncryptor = scope.ServiceProvider.GetRequiredService<IPasswordEncryptor>();
        await db.Database.EnsureCreatedAsync();

        var role = new Role { Name = "Admin", Description = "管理员" };
        // 添加角色种子数据
        if (!await db.Roles.AsNoTracking().AnyAsync(x => x.Name == "Admin"))
        {
            db.Roles.Add(role);
            await db.SaveChangesAsync();
        }

        // 添加用户种子数据
        if (await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == "admin") == null)
        {
            var pwd = passwordEncryptor.Encrypt("QWE@qwe123"); ;
            var user = new User { Name = "admin", Nickname = "超级管理员", Email = "123@qq.com", Password = pwd };
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
}
