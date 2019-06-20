using System;

namespace EventDispatcher
{
    /// <summary>
    /// The Event dispatcher. The event dispatcher is here to:
    /// <br />
    /// register event names to concrete implementations
    /// <br />
    /// Register a callback for a specific event to a event handler
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Register an event name to a concrete class 
        /// </summary>
        /// <param name="eventName">the string event name</param>
        /// <param name="concreteType">The concrete class for the inner event</param>
        void RegisterEvent(string eventName, Type concreteType);

        /// <summary>
        /// dispatcher.RegisterCallback&lt;Models.SomethingChanged&gt;((somethingChangedEvent) => { DO SOMETHING HERE WITH THE EVENT });
        /// All generic types are constrained to class
        /// </summary>
        /// <typeparam name="EventType"></typeparam>
        /// <param name="callback"></param>
        void RegisterCallback<EventType>(Action<EventType> callback) 
            where EventType : class;
    }
}
