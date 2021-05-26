using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delpin.API.Services
{
    public class AvailabilityHostedService : BackgroundService
    {
        private readonly ILogger<AvailabilityHostedService> _logger;
        private readonly IServiceProvider _services;
        private Timer _timer;

        public AvailabilityHostedService(ILogger<AvailabilityHostedService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }
        // Execute on background task for update on availability for product items and timer for how often it runs
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(8));

            return Task.CompletedTask;
        }
        // Makes a scope and calls method through scope to GenericRepositoryProcessingService class
        private async void DoWork(object state)
        {
            _logger.LogInformation("Availability Hosted Service starting");

            using (var scope = _services.CreateScope())
            {
                var repositoryService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await repositoryService.DoWork();
            }
        }
        // When availability Hosted Service is stopping
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Availability Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return base.StopAsync(cancellationToken);
        }
    }
}
