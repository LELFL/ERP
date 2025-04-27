#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 角色
/// </summary>
public partial class RoleGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<RoleGetListOutput>>
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// 角色描述
    /// </summary>
    public string? Description { get; set; }
    
}