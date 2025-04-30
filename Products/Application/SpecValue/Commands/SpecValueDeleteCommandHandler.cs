using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class SpecValueDeleteCommandHandler : IRequestHandler<SpecValueDeleteCommand>
{
    private readonly IRepository<SpecValue, long> _repository;

    public SpecValueDeleteCommandHandler(IRepository<SpecValue, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(SpecValueDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
