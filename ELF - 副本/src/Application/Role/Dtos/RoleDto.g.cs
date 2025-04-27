#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 角色
/// </summary>
public partial class RoleDto : BaseDto<long>
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }  = default!;
    
    /// <summary>
    /// 角色描述
    /// </summary>
    public string? Description { get; set; }  = default!;
    
    /// <summary>
    /// 角色权限集合
    /// </summary>
    public List<PermissionDto> Permissions { get; set; }  = new();
    
}