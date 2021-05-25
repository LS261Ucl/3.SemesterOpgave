using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delpin.Mvc.Services
{
    public class ShoppingCartHostedService : IHostedService, IDisposable
    {
        private const int ShoppingCartLifeSpanInHours = 3;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILogger<ShoppingCartHostedService> _logger;
        private int _executionCount = 0;
        private int _purgedShoppingCarts = 0;
        private Timer _timer;

        public ShoppingCartHostedService(IShoppingCartService shoppingCartService, ILogger<ShoppingCartHostedService> logger)
        {
            _shoppingCartService = shoppingCartService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Shopping Cart Hosted Services");
            _timer = new Timer(UpdateShoppingCarts, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private void UpdateShoppingCarts(object state)
        {
            Interlocked.Increment(ref _executionCount);

            var createdAtShoppingCarts = _shoppingCartService.GetCreatedAt();

            foreach (var cart in createdAtShoppingCarts.Where(cart =>
                cart.Value.AddHours(ShoppingCartLifeSpanInHours) < DateTime.UtcNow))
            {
                _shoppingCartService.RemoveShoppingCart(cart.Key);
                Interlocked.Increment(ref _purgedShoppingCarts);
            }

            _logger.LogInformation($"Shopping Cart Hosted service currently running at: {_executionCount} rounds & has purged: {_purgedShoppingCarts} shopping carts");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Shopping Cart Hosted service");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
