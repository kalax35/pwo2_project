using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PWO.API.Hubs;

namespace PWO.API.Worker
{
    public class NotificationWorker : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private AppDbContext _dbContext;

        public NotificationWorker(IHubContext<NotificationHub> hubContext, IServiceScopeFactory  serviceScopeFactory)
        {
            _hubContext = hubContext;
            _serviceScopeFactory= serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var notificationsToSend = await _dbContext.Notifications
                        .Where(x => !x.IsSent)
                        .ToListAsync();

                    foreach (var notification in notificationsToSend)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification.Message);

                        notification.IsSent = true;
                    }

                    //foreach (var notification in notificationsToSend)
                    //{
                    //    // Wysyłaj powiadomienia tylko do użytkownika przypisanego do każdego powiadomienia
                    //    await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification.Message);

                    //    // Oznacz powiadomienie jako wysłane
                    //    notification.IsSent = true;
                    //}


                    await _dbContext.SaveChangesAsync();

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
