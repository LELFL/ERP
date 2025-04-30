using ELF.Data;
using ELF.Infrastructure.Data;
using ELF.Infrastructure.Data.Interceptors;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Repositories;
using Services;
using Utils;
using Yitter.IdGenerator;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;
        var services = builder.Services;

        builder.AddRedisClient(connectionName: "cache");
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

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUser, CurrentUser>();

        // 配置OpenIddict
        services.AddOpenIddict()
            // 配置 OpenIddict 核心组件
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();
            });

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddSingleton(TimeProvider.System);

        services.AddScoped(typeof(IRepository<,>), typeof(EFCoreRepository<,>));
        services.AddScoped<IPasswordEncryptor, Md5PasswordEncryptor>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdentityService, IdentityService>();

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
}
