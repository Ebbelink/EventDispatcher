namespace EventDispatcher
{
    internal interface IEventCallback
    {
        void Invoke(object argument);
    }
}
