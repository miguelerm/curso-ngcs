using MassTransit;
using Messages;
using System;

namespace Broadcaster
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(config => {
                var host = config.Host(new Uri("rabbitmq://localhost/demos"), h => {
                    h.Username("demo-user");
                    h.Password("demo-user");
                });

                config.ReceiveEndpoint(host, "broadcast", endpoint =>
                {
                    endpoint.Consumer<BroadcastConsumer<ISayHello>>();
                });
            });

            bus.Start();

            Console.WriteLine("broadcast");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bus.Stop();
        }
    }
}
