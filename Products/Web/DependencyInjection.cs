using Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();


        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        ConfigureCors(builder);
        // 注册服务
        builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        ConfigureAuthentication(builder.Services, builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
    }

    private static void ConfigureCors(IHostApplicationBuilder builder)
    {
        var origins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>();
        if (origins is null || origins.Length == 0)
            return;

        // Add CORS policy (only if origins are configured)
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(origins);
                });
        });

    }

    public static IApplicationBuilder UseDefaultCors(this IApplicationBuilder app)
    {
        var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var origins = config.GetSection("CorsOrigins").Get<string[]>();
        if (origins is null || origins.Length == 0)
            return app;
        return app.UseCors();
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var encryptionKeyText = configuration["OpenIddict:EncryptionKey"];
        if (string.IsNullOrEmpty(encryptionKeyText))
        {
            throw new ArgumentNullException("OpenIddict:EncryptionKey", "请配置证书密钥！");
        }

        var identity_server_address = configuration["OpenIddict:Issuer"];
        if (string.IsNullOrEmpty(identity_server_address))
        {
            throw new ArgumentNullException("OpenIddict:Issuer", "请配置身份服务器地址！");
        }

        // 配置OpenIddict
        services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(identity_server_address);
                var encryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(encryptionKeyText));
                options.AddEncryptionKey(encryptionKey);
                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PermissionConstants.PolicyNames, policy =>
                policy.Requirements.Add(new PermissionRequirement()));
        });
    }
}
