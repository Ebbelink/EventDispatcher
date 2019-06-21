using System;
using System.Collections.Generic;
using System.Text;

namespace EventEndpointDispatcher
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class EventTypeAttribute : Attribute
    {
        public string EventName { get; set; }

        public EventTypeAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
