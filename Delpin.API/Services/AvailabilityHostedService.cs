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
        private CancellationToken _stoppingToken;

        public AvailabilityHostedService(ILogger<AvailabilityHostedService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Availability Hosted Service starting");

            using (var scope = _services.CreateScope())
            {
                var repositoryService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await repositoryService.DoWork(_stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Availability Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return base.StopAsync(cancellationToken);
        }
    }
}
