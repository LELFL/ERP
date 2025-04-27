using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

/// <summary>
/// 处理更新用户密码的命令。
/// </summary>
public class UserUpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
{
    private readonly IRepository<User, long> _repository;
    private readonly IUser _user;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public UserUpdatePasswordCommandHandler(IRepository<User, long> repository, IUser user, IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _user = user;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Password = _passwordEncryptor.Encrypt(request.NewPassword);

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
