using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class PermissionUpdateCommandHandler : IRequestHandler<PermissionUpdateCommand, PermissionDto>
{
    private readonly IRepository<Permission, long> _repository;

    public PermissionUpdateCommandHandler(IRepository<Permission, long> repository)
    {
        _repository = repository;
    }

    public async Task<PermissionDto> Handle(PermissionUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<PermissionDto>();
    }
}
