using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class UnitDeleteCommandHandler : IRequestHandler<UnitDeleteCommand>
{
    private readonly IRepository<Unit, long> _repository;

    public UnitDeleteCommandHandler(IRepository<Unit, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UnitDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
