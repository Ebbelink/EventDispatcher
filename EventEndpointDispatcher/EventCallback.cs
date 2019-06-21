using System;
using System.Threading.Tasks;

namespace EventEndpointDispatcher
{
    internal class EventCallback<EventType> : IEventCallback
        where EventType : class
    {
        private Action<EventType> _callback;

        public EventCallback(Action<EventType> callback)
        {
            _callback = callback;
        }

        public void Invoke(object argument)
        {
            Task.Run(() =>
            {
                _callback.Invoke(argument as EventType);
            });
        }
    }
}
