using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Services
{
    public class LoadingService : IHostedService
    {
        // IServiceScopeFactory is used to create a new scope, this allows 
        // resolving scoped services within a singleton service like LoadingService.
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // IInitializationService instance is used to signal when the 
        // application has finished its initial loading process.
        private readonly IInitializationService _initializationService;

        // The constructor receives all necessary dependencies through dependency injection.
        public LoadingService(IServiceScopeFactory serviceScopeFactory, IInitializationService initializationService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _initializationService = initializationService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Maximum number of attempts to load data before failing.
            const int maxRetryAttempts = 3;
            // Delay in seconds between each retry attempt.
            const int delayBetweenFailuresInSeconds = 2;

            // A new scope is created to resolve scoped services.
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                // The ISwapapiClient service is resolved from the created scope.
                var swapapiClient = scope.ServiceProvider.GetRequiredService<ISwapapiClient>();

                // Retry logic in case of failure during data loading.
                for (int retry = 0; retry < maxRetryAttempts; retry++)
                {
                    try
                    {
                        // Try to load and cache Star Wars characters from original trilogy.
                        await swapapiClient.GetCharactersFromOriginalTrilogyAsync();

                        // If the data loading succeeds, mark the initialization as complete 
                        // and break the retry loop.
                        _initializationService.InitializationComplete = true;  
                        break; 
                    }
                    catch (Exception ex)
                    {
                        // If loading fails and we have not exceeded the maximum number of retry attempts, 
                        // delay the execution before the next attempt.
                        if (retry < maxRetryAttempts - 1) 
                        {
                            await Task.Delay(TimeSpan.FromSeconds(delayBetweenFailuresInSeconds));
                        }
                        else
                        {
                            // If we have exceeded the maximum number of retry attempts, re-throw the exception.
                            throw; 
                        }
                    }
                }
            }
        }

        // StopAsync is part of IHostedService interface, called when the application 
        // is stopping. It's not used in this case so we just return a completed task.
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}