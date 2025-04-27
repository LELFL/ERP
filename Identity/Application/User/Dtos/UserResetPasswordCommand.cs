namespace Dtos;
/// <summary>
/// 用户重置密码指令。
/// </summary>
/// <param name="Id"> 用户Id </param>
public record UserResetPasswordCommand(long Id) : IRequest;
