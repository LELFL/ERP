using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class UserGetListQueryHandler : IRequestHandler<UserGetListQuery, AmisPagedOutput<UserGetListOutput>>
{
    private readonly IRepository<User, long> _repository;

    public UserGetListQueryHandler(IRepository<User, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<UserGetListOutput>> Handle(UserGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<User>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<UserGetListOutput>(
            totalCount,
            entities.Adapt<List<UserGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
