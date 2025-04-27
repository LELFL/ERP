using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class PermissionDeleteCommandHandler : IRequestHandler<PermissionDeleteCommand>
{
    private readonly IRepository<Permission, long> _repository;

    public PermissionDeleteCommandHandler(IRepository<Permission, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(PermissionDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
