using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class SpecValueGetQueryHandler : IRequestHandler<SpecValueQuery, SpecValueDto>
{
    private readonly IRepository<SpecValue, long> _repository;

    public SpecValueGetQueryHandler(IRepository<SpecValue, long> repository)
    {
        _repository = repository;
    }

    public async Task<SpecValueDto> Handle(SpecValueQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<SpecValueDto>(); 
    }
}