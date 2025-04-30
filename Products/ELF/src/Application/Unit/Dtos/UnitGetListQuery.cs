#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 计量单位
/// </summary>
public partial class UnitGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<UnitGetListOutput>>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }
    
}