namespace Dtos;
/// <summary>
/// �û������ɫ����
/// </summary>
/// <param name="Id">�û�Id</param>
/// <param name="RoleIds">Ҫ����Ľ�ɫ</param>
public record UserRolesCommand(long Id, string RoleIds) : IRequest;
