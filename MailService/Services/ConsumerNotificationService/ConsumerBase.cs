using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MailService.Services.ConsumerNotificationService
{
    public abstract class ConsumerBase : RabbitMqClientBase
    {
        private readonly ILogger<ConsumerBase> _logger;
        protected abstract string QueueName { get; }

        public ConsumerBase(
            ConnectionFactory connectionFactory,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionFactory, logger)
        {
            _logger = consumerLogger;
        }

        protected async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                dynamic data = JsonConvert.DeserializeObject<T>(body);

                await EmailService.EmailService.SendNotificationOfAssigment(data);
                _logger.LogInformation("Mail was successfully send.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
    }
}