using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand>
{
    private readonly IRepository<Product, long> _repository;

    public ProductDeleteCommandHandler(IRepository<Product, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
