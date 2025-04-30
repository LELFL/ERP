using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class ProductSKUUpdateCommandHandler : IRequestHandler<ProductSKUUpdateCommand, ProductSKUDto>
{
    private readonly IRepository<ProductSKU, long> _repository;

    public ProductSKUUpdateCommandHandler(IRepository<ProductSKU, long> repository)
    {
        _repository = repository;
    }

    public async Task<ProductSKUDto> Handle(ProductSKUUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<ProductSKUDto>();
    }
}
