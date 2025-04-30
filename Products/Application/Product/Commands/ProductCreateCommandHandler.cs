using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, ProductDto>
{
    private readonly IRepository<Product, long> _repository;

    public ProductCreateCommandHandler(IRepository<Product, long> repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<ProductDto>();
    }
}
