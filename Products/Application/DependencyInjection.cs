using ELF.Application.Common.Behaviours;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Services;
using System.Reflection;
using Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        var identity_server_address = configuration["OpenIddict:Issuer"];
        if (identity_server_address == null)
            throw new Exception("Identity Server address is not configured");
        services.AddHttpClient<IIdentityRemoteService, IdentityRemoteService>(o => o.BaseAddress = new(identity_server_address))
            .AddAuthToken();
        //services.AddTransient<IIdentityRemoteService, IdentityRemoteService>();
    }
}
