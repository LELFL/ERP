using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, UserDto>
{
    private readonly IRepository<User, long> _repository;

    public UserUpdateCommandHandler(IRepository<User, long> repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<UserDto>();
    }
}
