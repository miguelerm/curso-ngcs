using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abs.FilesManager.Services
{
    public class BusService : IHostedService
    {
        private readonly IBusControl busControl;
        private readonly IEnumerable<IConsumeObserver> observers;

        public BusService(IBusControl busControl, IEnumerable<IConsumeObserver> observers)
        {
            this.busControl = busControl;
            this.observers = observers;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var observer in observers)
            {
                busControl.ConnectConsumeObserver(observer);
            }

            return busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return busControl.StopAsync(cancellationToken);
        }
    }
}
