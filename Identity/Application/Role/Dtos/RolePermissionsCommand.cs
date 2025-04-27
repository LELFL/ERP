namespace Dtos;
public record RolePermissionsCommand(long Id, string PermissionIds) : IRequest;
