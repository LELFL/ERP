using Ardalis.GuardClauses;
using Dtos;
using Entities;
using Interfaces;

namespace Commands;

public class CategoryDeleteCommandHandler : IRequestHandler<CategoryDeleteCommand>
{
    private readonly IRepository<Category, long> _repository;

    public CategoryDeleteCommandHandler(IRepository<Category, long> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _repository.Remove(entity);

        await _repository.SaveChangesAsync(cancellationToken);
    }

}
