using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class PermissionCreateCommandHandler : IRequestHandler<PermissionCreateCommand, PermissionDto>
{
    private readonly IRepository<Permission,long> _repository;

    public PermissionCreateCommandHandler(IRepository<Permission, long> repository)
    {
        _repository = repository;
    }

    public async Task<PermissionDto> Handle(PermissionCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<PermissionDto>();
    }
}
