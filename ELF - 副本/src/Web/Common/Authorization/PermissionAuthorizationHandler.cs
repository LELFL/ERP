using System.Security.Claims;
using Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Microsoft.AspNetCore.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IIdentityService _identityService;

    public PermissionAuthorizationHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var httpContext = context.Resource as HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return;
        }

        // 从路由元数据中获取权限标识
        var endpoint = httpContext.GetEndpoint();
        var permissionMetadata = endpoint?.Metadata.GetMetadata<TagsAttribute>();
        if (permissionMetadata == null)
        {
            // 若路由未标记权限，默认允许访问
            context.Succeed(requirement);
            return;
        }

        var requiredPermission = permissionMetadata.Tags.FirstOrDefault();
        if (requiredPermission == null)
        {
            // 若路由未标记权限，默认允许访问
            context.Succeed(requirement);
            return;
        }

        // 获取用户ID
        var userIdValue = httpContext.User.FindFirstValue(Claims.Subject);
        if (string.IsNullOrEmpty(userIdValue) || !long.TryParse(userIdValue, out var userId))
        {
            context.Fail();
            return;
        }


        var success = await _identityService.AuthorizeAsync(userId, requiredPermission);
        // 验证权限
        if (success)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
