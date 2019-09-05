using MassTransit;
using Messages;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private static readonly Regex regex = new Regex("^([0-9]+)[sS]\\s(.*)$");
        private static readonly Uri schedulerUri = new Uri("rabbitmq://localhost/demos/masstransit_quartz_scheduler");
        private static readonly Uri broadcastUri = new Uri("rabbitmq://localhost/demos/broadcast");
        private static readonly Uri consoleAppUri = new Uri("rabbitmq://localhost/demos/console-app");

        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(config => {
                var host = config.Host(new Uri("rabbitmq://localhost/demos"), h => {
                    h.Username("demo-user");
                    h.Password("demo-user");
                });

                config.UseMessageScheduler(schedulerUri);
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

                if (salute.StartsWith("c: "))
                {
                    bus.ScheduleSend<ISayHello>(
                       consoleAppUri,
                       TimeSpan.FromSeconds(5),
                       new { Salute = salute }
                   );

                }
                else
                {
                    var parsed = regex.Match(salute);
                    if (parsed.Success)
                    {
                        var seconds = int.Parse(parsed.Groups[1].Value);
                        var message = parsed.Groups[2].Value;
                        bus.ScheduleSend<ISayHello>(
                            broadcastUri,
                            TimeSpan.FromSeconds(seconds),
                            new { Salute = message }
                        );
                    }
                    else
                    {
                        bus.Publish<ISayHello>(new { Salute = salute });
                    }
                }


            }

            bus.Stop();
        }
    }
}
