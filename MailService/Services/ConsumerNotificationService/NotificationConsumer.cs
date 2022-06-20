using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MailService.Services.ConsumerNotificationService
{
    public class NotificationConsumer : ConsumerBase, IHostedService
    {
        protected override string QueueName => "notifications";

        public NotificationConsumer(
            ConnectionFactory connectionFactory,
            ILogger<NotificationConsumer> notificationConsumerLogger,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionFactory, consumerLogger, logger)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += (sender, @event) => OnEventReceived<NotificationOfIssueAssignment>(sender, @event);
                Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                notificationConsumerLogger.LogCritical(ex, "Error while consuming message.");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}