using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class SpecValueUpdateCommandHandler : IRequestHandler<SpecValueUpdateCommand, SpecValueDto>
{
    private readonly IRepository<SpecValue, long> _repository;

    public SpecValueUpdateCommandHandler(IRepository<SpecValue, long> repository)
    {
        _repository = repository;
    }

    public async Task<SpecValueDto> Handle(SpecValueUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<SpecValueDto>();
    }
}
