using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class SpecGetQueryHandler : IRequestHandler<SpecQuery, SpecDto>
{
    private readonly IRepository<Spec, long> _repository;

    public SpecGetQueryHandler(IRepository<Spec, long> repository)
    {
        _repository = repository;
    }

    public async Task<SpecDto> Handle(SpecQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<SpecDto>(); 
    }
}