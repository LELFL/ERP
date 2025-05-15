using Interfaces;
using System.Net.Http.Json;

namespace Services;

internal class IdentityRemoteService(HttpClient httpClient) : IIdentityRemoteService
{
    private readonly string remoteServiceBaseUrl = "";

    public async Task<string[]> GetUserPermissionsAsync()
    {
        var uri = $"{remoteServiceBaseUrl}Users/Permissions";
        var result = await httpClient.GetFromJsonAsync<string[]>(uri);
        return result!;
    }
}