using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abs.FilesManager.Services.Observers
{
    public class LoggingObserver : IConsumeObserver
    {
        private readonly ILoggerFactory loggerFactory;

        public LoggingObserver(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            var logger = loggerFactory.CreateLogger<T>();
            logger.LogError(exception, "message fault: {@message}", context.CorrelationId, context.Message);
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            var logger = loggerFactory.CreateLogger<T>();
            logger.LogDebug("{conversationId}: {correlationId} message consumed: {@message} headers: {@headers}", context.ConversationId, context.CorrelationId ?? context.InitiatorId, context.Message, context.Headers.GetAll());
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
