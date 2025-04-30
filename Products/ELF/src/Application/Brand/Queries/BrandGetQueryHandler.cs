using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class BrandGetQueryHandler : IRequestHandler<BrandQuery, BrandDto>
{
    private readonly IRepository<Brand, long> _repository;

    public BrandGetQueryHandler(IRepository<Brand, long> repository)
    {
        _repository = repository;
    }

    public async Task<BrandDto> Handle(BrandQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<BrandDto>(); 
    }
}