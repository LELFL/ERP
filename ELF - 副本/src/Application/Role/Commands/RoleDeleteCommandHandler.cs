using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class RoleDeleteCommandHandler : IRequestHandler<RoleDeleteCommand>
{
    private readonly IRepository<Role, long> _repository;

    public RoleDeleteCommandHandler(IRepository<Role, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
