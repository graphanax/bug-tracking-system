namespace BugTracker.Services.ProducerNotificationService
{
    public interface IRabbitMqProducer<in T>
    {
        void Publish(T @event);
    }
}