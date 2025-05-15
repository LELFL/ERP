namespace Interfaces;

public interface IUser
{
    string? Id { get; }
    string? Name { get; }
    Task<string?> GetTokenAsync();
}
