namespace Microsoft.AspNetCore.Builder;
public static class WithPermissionExtensions
{

    public static RouteHandlerBuilder WithPermission(this RouteHandlerBuilder builder, string tags)
    {
        return builder.WithMetadata(new PermissionAttribute(tags));
    }
}
