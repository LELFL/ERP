
namespace ELF.Web.Infrastructure;

public static class MapEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        ELF.Web.Endpoints.Brands.Map(app);
        ELF.Web.Endpoints.Categorys.Map(app);
        ELF.Web.Endpoints.Products.Map(app);
        ELF.Web.Endpoints.ProductSKUs.Map(app);
        ELF.Web.Endpoints.Specs.Map(app);
        ELF.Web.Endpoints.SpecValues.Map(app);
        ELF.Web.Endpoints.Units.Map(app);
        return app;
    }
}
