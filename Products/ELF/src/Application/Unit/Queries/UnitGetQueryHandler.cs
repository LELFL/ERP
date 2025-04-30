using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class UnitGetQueryHandler : IRequestHandler<UnitQuery, UnitDto>
{
    private readonly IRepository<Unit, long> _repository;

    public UnitGetQueryHandler(IRepository<Unit, long> repository)
    {
        _repository = repository;
    }

    public async Task<UnitDto> Handle(UnitQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<UnitDto>(); 
    }
}