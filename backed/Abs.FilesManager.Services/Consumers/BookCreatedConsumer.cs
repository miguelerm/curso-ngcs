using Abs.Messages.BooksCatalog.Events;
using Abs.Messages.FilesManager.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Abs.FilesManager.Services.Consumers
{
    public class BookCreatedConsumer : IConsumer<IBookCreated>
    {
        private readonly ILogger<BookCreatedConsumer> logger;

        public BookCreatedConsumer(ILogger<BookCreatedConsumer> logger)
        {
            this.logger = logger;
        }
        public Task Consume(ConsumeContext<IBookCreated> context)
        {
            var message = context.Message;
            logger.LogDebug("*** New IBookCreated Message received ***");
            return Task.WhenAll(
                message.Covers
                    .Select(c => 
                        context.Publish<IPutTemporaryFile>(c)
                    )
            );
        }
    }
}
