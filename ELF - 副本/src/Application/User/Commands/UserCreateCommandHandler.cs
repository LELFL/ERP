using Constants;
using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, UserDto>
{
    private readonly IRepository<User,long> _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public UserCreateCommandHandler(IRepository<User, long> repository,IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task<UserDto> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);
        entity.Password = _passwordEncryptor.Encrypt(UserConsts.DefaultPassword);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<UserDto>();
    }
}
