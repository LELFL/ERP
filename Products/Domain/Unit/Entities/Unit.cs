namespace Entities;

/// <summary>
/// 计量单位
/// </summary>
public class Unit : BaseEntity<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;
}
