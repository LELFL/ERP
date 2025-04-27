using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class RoleGetQueryHandler : IRequestHandler<RoleQuery, RoleDto>
{
    private readonly IRepository<Role, long> _repository;

    public RoleGetQueryHandler(IRepository<Role, long> repository)
    {
        _repository = repository;
    }

    public async Task<RoleDto> Handle(RoleQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<RoleDto>(); 
    }
}