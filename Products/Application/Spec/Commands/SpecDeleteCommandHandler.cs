using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class SpecDeleteCommandHandler : IRequestHandler<SpecDeleteCommand>
{
    private readonly IRepository<Spec, long> _repository;

    public SpecDeleteCommandHandler(IRepository<Spec, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(SpecDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
