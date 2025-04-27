#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 角色
/// </summary>
public partial class RoleCreateCommand : IRequest<RoleDto>
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }  = default!;
    
    /// <summary>
    /// 角色描述
    /// </summary>
    public string? Description { get; set; }  = default!;
    
}