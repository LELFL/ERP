using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class SpecValueCreateCommandHandler : IRequestHandler<SpecValueCreateCommand, SpecValueDto>
{
    private readonly IRepository<SpecValue, long> _repository;

    public SpecValueCreateCommandHandler(IRepository<SpecValue, long> repository)
    {
        _repository = repository;
    }

    public async Task<SpecValueDto> Handle(SpecValueCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<SpecValueDto>();
    }
}
