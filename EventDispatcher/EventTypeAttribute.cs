using System;
using System.Collections.Generic;
using System.Text;

namespace EventDispatcher
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal class EventTypeAttribute : Attribute
    {
        public string EventName { get; set; }

        public EventTypeAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
