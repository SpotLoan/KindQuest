using KindQuestAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KindQuestAPI.BackgroundServices
{
    public class DailyQuestSchedulerService : BackgroundService
    {
        private readonly ILogger<DailyQuestSchedulerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DailyQuestSchedulerService(
            ILogger<DailyQuestSchedulerService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Daily Quest Scheduler Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Calculate time until next run (midnight UTC)
                var now = DateTime.UtcNow;
                var tomorrow = now.Date.AddDays(1);
                var timeUntilMidnight = tomorrow - now;

                _logger.LogInformation($"Next quest refresh scheduled in {timeUntilMidnight.Hours} hours and {timeUntilMidnight.Minutes} minutes");

                // Wait until the next scheduled time
                await Task.Delay(timeUntilMidnight, stoppingToken);

                // It's time to refresh quests
                try
                {
                    await RefreshQuestsAsync();
                    _logger.LogInformation("Daily quests refreshed successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while refreshing daily quests");
                }

                // Add a small delay to prevent tight loop if execution happens right at midnight
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task RefreshQuestsAsync()
        {
            _logger.LogInformation("Starting daily quest refresh for all users");

            // Create a new scope to resolve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                var questService = scope.ServiceProvider.GetRequiredService<QuestService>();
                
                int usersUpdated = await questService.AssignDailyQuestsToAllUsersAsync();
                
                _logger.LogInformation($"Daily quests refreshed for {usersUpdated} users");
            }
        }
    }
}