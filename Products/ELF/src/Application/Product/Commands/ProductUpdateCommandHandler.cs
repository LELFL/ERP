using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, ProductDto>
{
    private readonly IRepository<Product, long> _repository;

    public ProductUpdateCommandHandler(IRepository<Product, long> repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<ProductDto>();
    }
}
