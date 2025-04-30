using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class UnitUpdateCommandHandler : IRequestHandler<UnitUpdateCommand, UnitDto>
{
    private readonly IRepository<Unit, long> _repository;

    public UnitUpdateCommandHandler(IRepository<Unit, long> repository)
    {
        _repository = repository;
    }

    public async Task<UnitDto> Handle(UnitUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<UnitDto>();
    }
}
