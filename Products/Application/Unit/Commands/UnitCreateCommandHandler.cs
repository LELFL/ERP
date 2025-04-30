using Dtos;
using Interfaces;
using Mapster;

namespace Commands;

public class UnitCreateCommandHandler : IRequestHandler<UnitCreateCommand, UnitDto>
{
    private readonly IRepository<Unit, long> _repository;

    public UnitCreateCommandHandler(IRepository<Unit, long> repository)
    {
        _repository = repository;
    }

    public async Task<UnitDto> Handle(UnitCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<UnitDto>();
    }
}
