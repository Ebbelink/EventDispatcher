namespace EventEndpointDispatcher
{
    internal interface IEventCallback
    {
        void Invoke(object argument);
    }
}
