using Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Services;

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
