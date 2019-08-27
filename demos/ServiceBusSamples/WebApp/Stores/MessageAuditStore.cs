using MassTransit.Audit;
using System;
using System.Threading.Tasks;

namespace WebApp.Stores
{
    public class MessageAuditStore : IMessageAuditStore
    {
        public Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            Console.WriteLine("Message processed");
            return Task.CompletedTask;
        }
    }
}
