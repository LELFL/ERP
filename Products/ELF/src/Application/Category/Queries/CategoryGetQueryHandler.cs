using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class CategoryGetQueryHandler : IRequestHandler<CategoryQuery, CategoryDto>
{
    private readonly IRepository<Category, long> _repository;

    public CategoryGetQueryHandler(IRepository<Category, long> repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto> Handle(CategoryQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<CategoryDto>(); 
    }
}