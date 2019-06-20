using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventDispatcher
{
    internal class EventDispatcher : IEventDispatcher
    {
        private Dictionary<string, Type> _registeredMessageTypesToConcreteClass;
        private Dictionary<Type, List<IEventCallback>> _concreteMessageToRegisteredCallbacks;

        public EventDispatcher()
        {
            _registeredMessageTypesToConcreteClass = new Dictionary<string, Type>();
            _concreteMessageToRegisteredCallbacks = new Dictionary<Type, List<IEventCallback>>();
        }

        public void RegisterEvent(string eventName, Type concreteType)
        {
            _registeredMessageTypesToConcreteClass.Add(eventName, concreteType);
        }

        public void RegisterCallback<EventType>(Action<EventType> callback) where EventType : class
        {
            List<IEventCallback> registeredCallbacks;

            if (!_concreteMessageToRegisteredCallbacks.TryGetValue(typeof(EventType).GetType(), out registeredCallbacks))
            {
                registeredCallbacks = new List<IEventCallback>();
                _concreteMessageToRegisteredCallbacks.Add(typeof(EventType), registeredCallbacks);
            }
            registeredCallbacks.Add(new EventCallback<EventType>(callback));
        }

        internal Type GetEventType(string eventName)
        {
            return _registeredMessageTypesToConcreteClass[eventName];
        }

        internal void RaiseEvent<EventType>(EventType model)
        {
            List<IEventCallback> registeredCallbacks;

            if (_concreteMessageToRegisteredCallbacks.TryGetValue(model.GetType(), out registeredCallbacks))
            {
                foreach (var callback in registeredCallbacks)
                {
                    callback.Invoke(model);
                }
            }
        }
    }
}
