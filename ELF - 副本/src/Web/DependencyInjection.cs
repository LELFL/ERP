using Constants;
using ELF.Infrastructure.Data;
using ELF.Web.Services;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Web;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUser, CurrentUser>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();


        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        ConfigureCors(builder);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
        // 注册服务
        builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PermissionConstants.PolicyNames, policy =>
                policy.Requirements.Add(new PermissionRequirement()));
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddHostedService<Worker>();
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
}
