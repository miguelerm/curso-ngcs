using MassTransit;
using Messages;
using System;

namespace ConsoleApp
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
            });

            bus.Start();
            

            while(true)
            {
                Console.Write("Salute: ");
                var salute = Console.ReadLine();
                if (salute == "exit")
                {
                    break;
                }
                bus.Publish<ISayHello>(new { Salute = salute });
            }

            bus.Stop();
        }
    }
}
