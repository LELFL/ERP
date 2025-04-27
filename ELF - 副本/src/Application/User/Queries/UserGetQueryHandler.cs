using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class UserGetQueryHandler : IRequestHandler<UserQuery, UserDto>
{
    private readonly IRepository<User, long> _repository;

    public UserGetQueryHandler(IRepository<User, long> repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<UserDto>(); 
    }
}
