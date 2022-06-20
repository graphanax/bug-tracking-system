using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BugTracker.Services.ProducerNotificationService
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        protected readonly string NotifierExchange = "NotifierExchange";
        protected readonly string NotifierQueue = "notifications";
        protected const string NotifierQueueAndExchangeRoutingKey = "notification";

        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqClientBase> _logger;

        protected RabbitMqClientBase(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            ConnectToRabbitMq();
        }

        private void ConnectToRabbitMq()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel != null && Channel.IsOpen) return;
            
            Channel = _connection.CreateModel();
            
            Channel.ExchangeDeclare(
                exchange: NotifierExchange,
                type: "direct",
                durable: true,
                autoDelete: false);

            Channel.QueueDeclare(
                queue: NotifierQueue,
                durable: false,
                exclusive: false,
                autoDelete: false);

            Channel.QueueBind(
                queue: NotifierQueue,
                exchange: NotifierExchange,
                routingKey: NotifierQueueAndExchangeRoutingKey);
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
            }
            
            GC.SuppressFinalize(this);
        }
    }
}