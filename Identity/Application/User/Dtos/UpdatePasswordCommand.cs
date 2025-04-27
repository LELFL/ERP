namespace Dtos;
/// <summary>
/// 更新密码命令
/// </summary>
/// <param name="Id"> 用户Id </param>
/// <param name="Password"> 密码 </param>
public record UpdatePasswordRequest(string OldPassword, string NewPassword, string ConfirmNewPassword) : IRequest;
/// <summary>
/// 更新密码命令
/// </summary>
/// <param name="Id"> 用户Id </param>
/// <param name="Password"> 密码 </param>
public record UpdatePasswordCommand(long Id, string OldPassword, string NewPassword) : IRequest;
