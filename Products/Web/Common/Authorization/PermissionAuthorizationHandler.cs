using Constants;
using Interfaces;

namespace Microsoft.AspNetCore.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IIdentityService _identityService;
    private readonly IUser _user;

    public PermissionAuthorizationHandler(IIdentityService identityService, IUser user)
    {
        _identityService = identityService;
        this._user = user;
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

        var userIdValue = _user.Id;
        if (string.IsNullOrEmpty(userIdValue) || !long.TryParse(userIdValue, out var userId))
        {
            // 未登录，直接拒绝访问
            context.Fail();
            return;
        }

        if (_user.Name == UserConsts.AdminUserName)
        {
            // 如果是管理员，则直接允许访问
            context.Succeed(requirement);
            return;
        }

        // 从路由元数据中获取权限标识
        var endpoint = httpContext.GetEndpoint();
        var permissionMetadata = endpoint?.Metadata.GetMetadata<PermissionAttribute>();
        if (permissionMetadata == null)
        {
            // 若路由未标记权限，默认允许访问
            context.Succeed(requirement);
            return;
        }

        var requiredPermission = permissionMetadata.Code;
        if (requiredPermission == null)
        {
            // 若路由未标记权限，默认允许访问
            context.Succeed(requirement);
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
