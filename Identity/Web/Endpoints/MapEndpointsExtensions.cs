using ELF.Endpoints;
using ELF.Web.Endpoints;

namespace Microsoft.AspNetCore.Builder;

public static class MapEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        Users.Map(app);
        Permissions.Map(app);
        Roles.Map(app);
        Auth.Map(app);
        return app;
    }
}
