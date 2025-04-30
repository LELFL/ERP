using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class BrandUpdateCommandHandler : IRequestHandler<BrandUpdateCommand, BrandDto>
{
    private readonly IRepository<Brand, long> _repository;

    public BrandUpdateCommandHandler(IRepository<Brand, long> repository)
    {
        _repository = repository;
    }

    public async Task<BrandDto> Handle(BrandUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Update(request.Id, request);
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Adapt<BrandDto>();
    }
}
