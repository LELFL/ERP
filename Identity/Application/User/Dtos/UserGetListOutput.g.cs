#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 用户
/// </summary>
public partial class UserGetListOutput : BaseDto<long>
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string Name { get; set; }  = default!;
    
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }  = default!;
    
    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; } 
    
    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } 
    
    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; }  = true;
    
}