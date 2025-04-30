#nullable enable
// 自动生成代码，请不要修改
namespace Dtos;

/// <summary>
/// 商品分类
/// </summary>
public partial class CategoryDto : BaseDto<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }  = default!;
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    public long? ImageId { get; set; } 
    
    /// <summary>
    /// 排序方式
    /// </summary>
    public int Sort { get; set; }  = 0;
    
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; }  = true;
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    public long? ParentId { get; set; } 
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    public CategoryDto? Parent { get; set; } 
    
    /// <summary>
    /// 子分类
    /// </summary>
    public List<CategoryDto> Children { get; set; }  = [];
    
}