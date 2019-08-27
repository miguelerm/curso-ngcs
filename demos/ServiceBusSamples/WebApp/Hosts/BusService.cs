using MassTransit;
using MassTransit.Audit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Hosts
{
    public class BusService : IHostedService
    {
        private readonly IBusControl busControl;
        private readonly IMessageAuditStore auditStore;

        public BusService(IBusControl busControl, IMessageAuditStore auditStore)
        {
            this.busControl = busControl;
            this.auditStore = auditStore;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            busControl.ConnectConsumeAuditObserver(auditStore);
            return busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return busControl.StopAsync(cancellationToken);
        }
    }
}
