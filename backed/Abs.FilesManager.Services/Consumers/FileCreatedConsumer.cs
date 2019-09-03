using System.Threading.Tasks;
using Abs.Messages.FilesManager.Events;
using MassTransit;

namespace Abs.FilesManager.Services.Consumers
{
    public class FileCreatedConsumer : IConsumer<IFileCreated>
    {
        public Task Consume(ConsumeContext<IFileCreated> context)
        {
            return context.ConsumeCompleted;
        }
    }
}
