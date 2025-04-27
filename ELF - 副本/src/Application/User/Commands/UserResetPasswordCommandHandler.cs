using Ardalis.GuardClauses;
using Constants;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

/// <summary>
/// 处理用户的重置密码命令。
/// </summary>
public class UserResetPasswordCommandHandler : IRequestHandler<UserResetPasswordCommand>
{
    private readonly IRepository<User, long> _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public UserResetPasswordCommandHandler(IRepository<User, long> repository,IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Password = _passwordEncryptor.Encrypt(UserConsts.DefaultPassword);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
