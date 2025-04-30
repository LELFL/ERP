#nullable enable
// 自动生成代码，请不要修改
using ELF.Common.Models;
namespace Dtos;

/// <summary>
/// 规格
/// </summary>
public partial class SpecGetListQuery : AmisGetListInputBase, IRequest<AmisPagedOutput<SpecGetListOutput>>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

}