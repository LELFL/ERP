namespace Entities;

/// <summary>
/// 商品
/// </summary>
public class Product : BaseEntity<long>
{
    /// <summary>
    /// 编号
    /// </summary>
    public string No { get; set; } = default!;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 分类ID
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 品牌ID
    /// </summary>
    public long BrandId { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 款式
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// 型号
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// 规格
    /// </summary>
    public string? Specification { get; set; }

    /// <summary>
    /// 单位ID
    /// </summary>
    public long? UnitId { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// 品牌
    /// </summary>
    public Brand? Brand { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public Unit? Unit { get; set; } = default!;

    /// <summary>
    /// 规格集合
    /// </summary>
    public virtual List<Spec> Specs { get; set; } = [];
}
