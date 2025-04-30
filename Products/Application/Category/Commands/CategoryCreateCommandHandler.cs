using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, CategoryDto>
{
    private readonly IRepository<Category, long> _repository;

    public CategoryCreateCommandHandler(IRepository<Category, long> repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<CategoryDto>();
    }
}
