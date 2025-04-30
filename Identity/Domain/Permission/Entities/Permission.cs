
namespace Entities;

/// <summary>
/// 权限
/// </summary>
public class Permission : BaseAuditableEntity<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = default!;
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 父级权限ID
    /// </summary>
    public Permission? Parent { get; set; }

    /// <summary>
    /// 父级权限
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 子权限
    /// </summary>
    public List<Permission> Children { get; set; } = [];
}
