using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class CategoryUpdateCommandHandler : IRequestHandler<CategoryUpdateCommand, CategoryDto>
{
    private readonly IRepository<Category, long> _repository;

    public CategoryUpdateCommandHandler(IRepository<Category, long> repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<CategoryDto>();
    }
}
