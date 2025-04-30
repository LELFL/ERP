namespace Entities;

/// <summary>
/// 用户
/// </summary>
public class User : BaseAuditableEntity<long>
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = default!;

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
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<Role> Roles { get; set; } = new();
}
