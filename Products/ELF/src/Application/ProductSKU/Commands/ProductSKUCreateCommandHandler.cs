using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class ProductSKUCreateCommandHandler : IRequestHandler<ProductSKUCreateCommand, ProductSKUDto>
{
    private readonly IRepository<ProductSKU,long> _repository;

    public ProductSKUCreateCommandHandler(IRepository<ProductSKU, long> repository)
    {
        _repository = repository;
    }

    public async Task<ProductSKUDto> Handle(ProductSKUCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<ProductSKUDto>();
    }
}
