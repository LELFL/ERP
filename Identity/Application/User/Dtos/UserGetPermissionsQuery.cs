namespace Dtos;
public record UserGetPermissionsQuery(long Id) : IRequest<string[]>;
