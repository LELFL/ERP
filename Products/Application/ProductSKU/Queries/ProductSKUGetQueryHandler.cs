using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class ProductSKUGetQueryHandler : IRequestHandler<ProductSKUQuery, ProductSKUDto>
{
    private readonly IRepository<ProductSKU, long> _repository;

    public ProductSKUGetQueryHandler(IRepository<ProductSKU, long> repository)
    {
        _repository = repository;
    }

    public async Task<ProductSKUDto> Handle(ProductSKUQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<ProductSKUDto>();
    }
}