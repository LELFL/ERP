namespace Entities;

/// <summary>
/// 商品品牌
/// </summary>
public class Brand : BaseEntity<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;
}
