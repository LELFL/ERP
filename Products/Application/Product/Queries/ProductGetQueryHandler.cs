using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class ProductGetQueryHandler : IRequestHandler<ProductQuery, ProductDto>
{
    private readonly IRepository<Product, long> _repository;

    public ProductGetQueryHandler(IRepository<Product, long> repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(ProductQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<ProductDto>();
    }
}