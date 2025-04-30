using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Commands;

public class BrandCreateCommandHandler : IRequestHandler<BrandCreateCommand, BrandDto>
{
    private readonly IRepository<Brand,long> _repository;

    public BrandCreateCommandHandler(IRepository<Brand, long> repository)
    {
        _repository = repository;
    }

    public async Task<BrandDto> Handle(BrandCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _repository.Add(request);

        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Adapt<BrandDto>();
    }
}
