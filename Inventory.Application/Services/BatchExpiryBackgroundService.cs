using Inventory.Application.Features.ProductBatches.Commands.CheckExpiring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace Inventory.Application.Services;

// Background worker running daily at midnight (00:00 UTC)
public class BatchExpiryBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BatchExpiryBackgroundService> _logger;

    public BatchExpiryBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<BatchExpiryBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Batch Expiry Background Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = now.Date.AddDays(1); // Next midnight
            var delay = nextRun - now;

            _logger.LogInformation("Next expiry check scheduled at: {NextRun}", nextRun);
            await Task.Delay(delay, stoppingToken);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

                _logger.LogInformation("Executing batch expiry and auto-markdown evaluation...");
                await bus.InvokeAsync(new CheckExpiringBatchesCommand(), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while evaluating batch expiry.");
            }
        }
    }
}