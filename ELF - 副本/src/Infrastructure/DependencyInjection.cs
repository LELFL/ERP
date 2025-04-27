using ELF.Data;
using ELF.Infrastructure.Data;
using ELF.Infrastructure.Data.Interceptors;
using ELF.Interfaces.Permissions;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Repositories;
using Services;
using Utils;
using Yitter.IdGenerator;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        ConfigureIdGenerator();

        var connectionString = configuration.GetConnectionString("identityserviceDb");
        Guard.Against.Null(connectionString, message: "Connection string 'identityserviceDb' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        var version = MySqlServerVersion.LatestSupportedServerVersion; //最近版本
        if (!EF.IsDesignTime)
        {
            Console.WriteLine(version.ToString());
            version = ServerVersion.AutoDetect(connectionString);
        }

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseMySql(connectionString, version);
            //注册OpenIddict所需的实体集。
            //注意：如果需要替换默认的OpenIddict实体，请使用通用重载。
            options.UseOpenIddict();
        });

        ConfigureAuthentication(services, configuration);

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddSingleton(TimeProvider.System);

        services.AddScoped(typeof(IRepository<,>), typeof(EFCoreRepository<,>));
        services.AddScoped<IPasswordEncryptor, Md5PasswordEncryptor>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddSingleton<ISeedDataService, SeedDataService>();

    }

    private static void ConfigureIdGenerator()
    {
        // 创建 IdGeneratorOptions 对象，可在构造函数中输入 WorkerId：
        var options = new IdGeneratorOptions(1);
        // options.WorkerIdBitLength = 10; // 默认值6，限定 WorkerId 最大值为2^6-1，即默认最多支持64个节点。
        // options.SeqBitLength = 6; // 默认值6，限制每毫秒生成的ID个数。若生成速度超过5万个/秒，建议加大 SeqBitLength 到 10。
        // options.BaseTime = Your_Base_Time; // 如果要兼容老系统的雪花算法，此处应设置为老系统的BaseTime。
        // ...... 其它参数参考 IdGeneratorOptions 定义。

        // 保存参数（务必调用，否则参数设置不生效）：
        YitIdHelper.SetIdGenerator(options);

        // 以上过程只需全局一次，且应在生成ID之前完成。
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var IssuerSigningKey = configuration["JWT:IssuerSigningKey"];
        var EncryptionKey = configuration["JWT:EncryptionKey"];
        if (string.IsNullOrEmpty(IssuerSigningKey)
            || string.IsNullOrEmpty(EncryptionKey))
        {
            throw new ArgumentNullException(nameof(IssuerSigningKey), "请配置证书密钥！");
        }

        // 配置OpenIddict
        services.AddOpenIddict()
            // 配置 OpenIddict 核心组件
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();
            })
            // 配置 OpenIddict 服务器
            .AddServer(options =>
            {
                #region 模式
                //1.授权码模式
                options.AllowAuthorizationCodeFlow();
                //2. 简化模式
                //     适用于无法进行客户端认证的客户端，如纯前端应用。由于安全性较低，这种模式已不再推荐使用。
                //3. 密码模式
                options.AllowPasswordFlow();
                //4. 客户端凭据模式
                options.AllowClientCredentialsFlow();
                #endregion
                options.AllowRefreshTokenFlow();
                //options.UseReferenceAccessTokens();

                //// Accept anonymous clients (i.e clients that don't send a client_id).
                //options.AcceptAnonymousClients();

                #region 证书
                #endregion

                #region 签名密钥
                options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
                #endregion

                #region 其他必要的配置
                //token过期时间
                options.SetAccessTokenLifetime(TimeSpan.FromSeconds(300));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(3));

                // 设置令牌和授权端点
                options.SetTokenEndpointUris("/connect/token")
                       .SetAuthorizationEndpointUris("/connect/authorize")
                       .SetUserInfoEndpointUris("/connect/userinfo")
                       ;
                #endregion
                ;

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
                //// 配置受众验证
                //options.AddAudiences("api");
            });

    }
}
