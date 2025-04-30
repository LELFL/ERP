using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class SpecUpdateCommandHandler : IRequestHandler<SpecUpdateCommand, SpecDto>
{
    private readonly IRepository<Spec, long> _repository;

    public SpecUpdateCommandHandler(IRepository<Spec, long> repository)
    {
        _repository = repository;
    }

    public async Task<SpecDto> Handle(SpecUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<SpecDto>();
    }
}
