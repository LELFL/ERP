namespace Dtos;
public record ProductQuery(long Id) : IRequest<ProductDto>;