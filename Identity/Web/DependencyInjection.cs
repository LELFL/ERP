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
        ConfigureAuthentication(builder.Services, builder.Configuration);

        ConfigureCors(builder);
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

        // 配置OpenIddict
        services.AddOpenIddict()
            .AddServer(options =>
            {
                #region 授权端点
                options.SetTokenEndpointUris("/connect/token")
                       .SetAuthorizationEndpointUris("/connect/authorize");
                #endregion

                #region 模式
                options.AllowAuthorizationCodeFlow();//授权码模式
                options.AllowPasswordFlow();//密码模式
                options.AllowClientCredentialsFlow();//客户端凭据模式
                options.AllowRefreshTokenFlow();
                //options.UseReferenceAccessTokens();
                options.AcceptAnonymousClients();
                #endregion

                #region 签名密钥
                var encryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(encryptionKeyText));
                options.AddEncryptionKey(encryptionKey);
                options.AddDevelopmentSigningCertificate();
                #endregion

                #region 其他必要的配置
                options.SetAccessTokenLifetime(TimeSpan.FromSeconds(300));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(3));
                #endregion

                options.UseAspNetCore().EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
        // 注册服务
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PermissionConstants.PolicyNames, policy =>
                policy.Requirements.Add(new PermissionRequirement()));
        });
    }
}
