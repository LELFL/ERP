using System.Security.Claims;
using Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ELF.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(Claims.Subject);

    public string? Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(Claims.Name);
}
