namespace Entities;
/// <summary>
/// 角色
/// </summary>
public class Role : BaseAuditableEntity<long>
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 角色权限集合
    /// </summary>
    public List<Permission> Permissions { get; set; } = [];
}
