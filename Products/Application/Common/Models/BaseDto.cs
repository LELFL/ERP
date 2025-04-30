namespace Dtos;

public class BaseDto<TKey>
{
    public TKey Id { get; set; } = default!;
}
