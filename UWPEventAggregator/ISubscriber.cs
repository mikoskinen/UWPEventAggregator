namespace UWPEventAggregator
{
    public interface ISubscriber<in TMessage>
    {
        void HandleMessage(TMessage message);
    }
}