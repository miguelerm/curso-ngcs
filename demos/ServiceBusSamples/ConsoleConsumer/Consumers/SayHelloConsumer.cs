using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleConsumer.Consumers
{
    public class SayHelloConsumer : IConsumer<ISayHello>
    {
        private static int count = 0;
        private static readonly object locker = new object();

        public Task Consume(ConsumeContext<ISayHello> context)
        {
            lock(locker)
            {
                count++;
            }

            Console.WriteLine($"Messages: {count}");
            return Task.CompletedTask;
        }
    }
}
