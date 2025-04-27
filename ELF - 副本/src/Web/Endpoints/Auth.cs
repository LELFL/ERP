using System.Security.Claims;
using Entities;
using Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Repositories;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ELF.Endpoints;

public class Auth
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/connect/token", LoginAysnc)
            .WithName($"{nameof(LoginAysnc)}")
            .WithDisplayName("获取权限详情");
    }

    public static async Task<IResult> LoginAysnc(
        ISender sender,
        IHttpContextAccessor httpContextAccessor,
        IPasswordEncryptor passwordEncryptor,
        IOpenIddictScopeManager scopeManager,
        IUserRepository userRepository)
    {
        var request = httpContextAccessor.HttpContext?.GetOpenIddictServerRequest() ?? throw new InvalidOperationException();

        User? user = null;
        ClaimsIdentity? identity = null;
        if (request.IsPasswordGrantType())
        {
            if (request.Password is null)
            {
                throw new InvalidOperationException("Password is null");
            }

            user = await userRepository.FirstOrDefaultAsync(x => x.Name == request.Username);

            if (user == null)
            {
                return Results.Problem("用户不存在", null, 403, "登录失败", Errors.InvalidGrant);
            }

            if (user.IsActive == false)
            {
                return Results.Problem("用户不被允许登录", null, 403, "登录失败", Errors.InvalidGrant);
            }

            var encryptorPwd = passwordEncryptor.Encrypt(request.Password);
            if (encryptorPwd != user.Password)
            {
                return Results.Problem("用户名或密码不正确", null, 403, "登录失败", Errors.InvalidGrant);
            }

            identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            //// Add the claims that will be persisted in the tokens.
            identity.SetClaim(Claims.Subject, user.Id.ToString())
                    .SetClaim(Claims.Name, user.Name)
                    .SetClaim(Claims.Nickname, user.Nickname)
                    ;
            identity.SetScopes(request.GetScopes());
            identity.SetDestinations(GetDestinations);
                    ;
        }

        else if (request.IsRefreshTokenGrantType())
        {
            // Retrieve the claims principal stored in the refresh token.
            var result = await httpContextAccessor.HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Retrieve the user profile corresponding to the refresh token.
            user = await userRepository.GetAsync(long.Parse(result.Principal!.GetClaim(Claims.Subject)!));

            if (user == null)
            {
                return Results.Problem("用户不存在", null, 403, "登录失败", Errors.InvalidGrant);
            }

            if (user.IsActive == false)
            {
                return Results.Problem("用户不被允许登录", null, 403, "登录失败", Errors.InvalidGrant);
            }

            //创建用户主体
            identity = new ClaimsIdentity(result.Principal?.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);
        }
        else
        {
            return Results.Problem("指定的授权类型未实现。", null, 403, "登录失败", Errors.InvalidGrant);
        }

        identity.SetDestinations(GetDestinations);

        return Results.SignIn(new ClaimsPrincipal(identity), null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}
