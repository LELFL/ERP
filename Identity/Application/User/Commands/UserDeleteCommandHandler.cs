using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand>
{
    private readonly IRepository<User, long> _repository;

    public UserDeleteCommandHandler(IRepository<User, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
