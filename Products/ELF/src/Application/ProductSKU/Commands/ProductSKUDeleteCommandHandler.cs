using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class ProductSKUDeleteCommandHandler : IRequestHandler<ProductSKUDeleteCommand>
{
    private readonly IRepository<ProductSKU, long> _repository;

    public ProductSKUDeleteCommandHandler(IRepository<ProductSKU, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(ProductSKUDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
