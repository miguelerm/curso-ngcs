using MassTransit;
using System.Threading.Tasks;

namespace Broadcaster
{
    public class BroadcastConsumer<T> : IConsumer<T> where T: class
    {
        public Task Consume(ConsumeContext<T> context)
        {
            var scheduled = context.Headers.Get<string>("MT-Quartz-Scheduled");
            var published = context.Headers.Get("_published", false as bool?);
            if (string.IsNullOrWhiteSpace(scheduled))
            {
                return Task.CompletedTask;
            }
            if (published.HasValue && published.Value)
            {
                return Task.CompletedTask;
            }
            return context.Publish(context.Message, pc => pc.Headers.Set("_published", true), context.CancellationToken);
        }
    }
}
