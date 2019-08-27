using ConsoleConsumer.Consumers;
using MassTransit;
using System;

namespace ConsoleConsumer
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

                config.ReceiveEndpoint(host, "console-app", endpoint =>
                {
                    endpoint.Consumer<SayHelloConsumer>();
                });
            });

            bus.Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bus.Stop();

        }
    }
}
