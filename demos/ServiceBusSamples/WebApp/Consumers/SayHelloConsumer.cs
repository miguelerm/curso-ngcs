using MassTransit;
using Messages;
using System;
using System.Threading.Tasks;

namespace WebApp.Consumers
{
    public class SayHelloConsumer : IConsumer<ISayHello>
    {
        public Task Consume(ConsumeContext<ISayHello> context)
        {
            Console.WriteLine($"Hello: {context.Message.Salute}");
            return Task.CompletedTask;
        }
    }
}
