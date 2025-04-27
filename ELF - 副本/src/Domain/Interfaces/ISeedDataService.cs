namespace Interfaces;
public interface ISeedDataService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
