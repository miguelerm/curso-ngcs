using System.Collections.Generic;
using System.Threading.Tasks;
using Abs.Messages.FilesManager.Events;
using Abs.Notifications.Service.Hubs;
using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Abs.FilesManager.Services.Consumers
{
    public class FileCreatedConsumer : IConsumer<IFileCreated>
    {
        private readonly IReadOnlyList<IHubProtocol> protocols = new IHubProtocol[] { new JsonHubProtocol() };
        public Task Consume(ConsumeContext<IFileCreated> context)
        {
            return context.Publish<All<NotificationsHub>>(new
            {
                Messages = protocols.ToProtocolDictionary("Message", 
                    new object[] {
                        new {
                            type = "file-created",
                            file = context.Message
                        }
                    })
            });
        }
    }
}

namespace Abs.Notifications.Service.Hubs
{
    public class NotificationsHub : Hub
    {
        // Actual implementation in the other project, but MT Needs the hub for the generic message type
    }
}
