namespace Entities;

/// <summary>
/// 规格
/// </summary>
public class Spec : BaseEntity<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 规格值集合
    /// </summary>
    public virtual List<SpecValue> SpecValues { get; set; } = [];
}
