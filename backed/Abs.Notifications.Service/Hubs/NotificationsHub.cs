using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Abs.Notifications.Service.Hubs
{
    [Authorize]
    public class NotificationsHub: Hub
    {
        private readonly ILogger<NotificationsHub> logger;

        public NotificationsHub(ILogger<NotificationsHub> logger)
        {
            this.logger = logger;
        }

        public Task Message(object data)
        {
            logger.LogDebug("User: {@user}", Context.User);
            return Clients.Others.SendAsync("message", data);
        }

        public override Task OnConnectedAsync()
        {
            logger.LogDebug("New connection opened");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            logger.LogWarning(exception, "Connection finished");
            return base.OnDisconnectedAsync(exception);
        }
    }
}