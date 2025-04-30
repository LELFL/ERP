using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class SpecCreateCommandHandler : IRequestHandler<SpecCreateCommand, SpecDto>
{
    private readonly IRepository<Spec, long> _repository;

    public SpecCreateCommandHandler(IRepository<Spec, long> repository)
    {
        _repository = repository;
    }

    public async Task<SpecDto> Handle(SpecCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<SpecDto>();
    }
}
