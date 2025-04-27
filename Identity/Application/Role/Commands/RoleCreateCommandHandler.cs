using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class RoleCreateCommandHandler : IRequestHandler<RoleCreateCommand, RoleDto>
{
    private readonly IRepository<Role,long> _repository;

    public RoleCreateCommandHandler(IRepository<Role, long> repository)
    {
        _repository = repository;
    }

    public async Task<RoleDto> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<RoleDto>();
    }
}
