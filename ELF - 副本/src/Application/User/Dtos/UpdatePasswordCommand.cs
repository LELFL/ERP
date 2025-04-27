namespace Dtos;
/// <summary>
/// ������������
/// </summary>
/// <param name="Id"> �û�Id </param>
/// <param name="Password"> ���� </param>
public record UpdatePasswordRequest(string OldPassword, string NewPassword, string ConfirmNewPassword) : IRequest;
/// <summary>
/// ������������
/// </summary>
/// <param name="Id"> �û�Id </param>
/// <param name="Password"> ���� </param>
public record UpdatePasswordCommand(long Id, string OldPassword, string NewPassword) : IRequest;
