using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class BrandDeleteCommandHandler : IRequestHandler<BrandDeleteCommand>
{
    private readonly IRepository<Brand, long> _repository;

    public BrandDeleteCommandHandler(IRepository<Brand, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(BrandDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
