using Interfaces;

namespace Web;

public class Worker : BackgroundService
{
    public Worker(ISeedDataService seedDataService)
    {
        _seedDataService = seedDataService;
    }
    private readonly ISeedDataService _seedDataService;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await _seedDataService.ExecuteAsync(cancellationToken);
    }
}
