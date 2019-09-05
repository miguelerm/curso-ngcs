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
            if (context.Message.Salute.Contains("Error 2"))
            {
                Console.WriteLine(":'(");
                throw new InvalidOperationException("Error");
            }

            if (context.Message.Salute.Contains("Error"))
            {
                Console.WriteLine(":(");
                throw new Exception("Error");
            }           

            Console.WriteLine($"Hello: {context.Message.Salute}");
            return Task.CompletedTask;
        }
    }
}
