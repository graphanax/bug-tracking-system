using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BugTracker.Services.ProducerNotificationService
{
    public class NotificationProducer : ProducerBase<NotificationOfIssueAssignment>
    {
        public NotificationProducer(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<NotificationOfIssueAssignment>> producerBaseLogger) :
            base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "NotifierExchange";
        protected override string RoutingKeyName => "notification";
        protected override string AppId => "NotificationProducer";
    }
}