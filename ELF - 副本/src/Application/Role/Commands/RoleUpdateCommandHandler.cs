using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class RoleUpdateCommandHandler : IRequestHandler<RoleUpdateCommand, RoleDto>
{
    private readonly IRepository<Role, long> _repository;

    public RoleUpdateCommandHandler(IRepository<Role, long> repository)
    {
        _repository = repository;
    }

    public async Task<RoleDto> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<RoleDto>();
    }
}
