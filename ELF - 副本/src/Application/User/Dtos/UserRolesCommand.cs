namespace Dtos;
/// <summary>
/// 用户分配角色命令
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="RoleIds">要分配的角色</param>
public record UserRolesCommand(long Id, string RoleIds) : IRequest;
