namespace Dtos;

/// <summary>
/// 权限
/// </summary>
public partial class PermissionGetListOutput
{
    /// <summary>
    /// 父级权限
    /// </summary>
    public string? ParentName { get; set; }
    /// <summary>
    /// 子权限列表
    /// </summary>
    public List<PermissionGetListOutput> Children { get; set; } = new();
}
