namespace Dtos;
public record UserQuery(long Id) : IRequest<UserDto>;