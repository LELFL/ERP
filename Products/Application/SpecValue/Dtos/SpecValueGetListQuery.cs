#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 规格值
/// </summary>
public partial class SpecValueGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<SpecValueGetListOutput>>
{
    /// <summary>
    /// 规格值
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// 规格ID
    /// </summary>
    public long? SpecId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }

}