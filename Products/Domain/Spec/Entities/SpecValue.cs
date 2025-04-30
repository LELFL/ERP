namespace Entities;

/// <summary>
/// 规格值
/// </summary>
public class SpecValue : BaseEntity<long>
{
    /// <summary>
    /// 规格值
    /// </summary>
    public string Value { get; set; } = default!;

    /// <summary>
    /// 规格ID
    /// </summary>
    public long? SpecId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } = 0;

    /// <summary>
    /// 规格
    /// </summary>
    public Spec? Spec { get; set; }
}
