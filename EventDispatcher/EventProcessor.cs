using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace EventDispatcher
{
    internal class EventProcessor : IEventProcessor
    {
        private const string _internalEventNameJPathIdentifier = "event";
        private const string _eventTypeJPathIdentifier = "type";

        private readonly EventDispatcher _dispatcher;

        /// <summary>
        /// Constructor for the EventProcessor
        /// </summary>
        /// <param name="dispatcher"></param>
        public EventProcessor(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher as EventDispatcher;
        }

        /// <summary>
        /// Read the body from the <see cref="HttpContext"/> and extract the internal event from it.
        /// Parse that internal event to a concrete type and raise the callbacks for the event
        /// </summary>
        /// <param name="context"></param>
        public void ProcessEvent(HttpContext context)
        {
            StreamReader sr = new StreamReader(context.Request.Body);

            context.Request.Body.Position = 0;

            string bodyContents = sr.ReadToEnd();

            sr.Dispose();

            var parsed = JObject.Parse(bodyContents);

            var eventType = parsed.SelectToken(_eventTypeJPathIdentifier);

            Type type = _dispatcher.GetEventType(eventType.Value<string>());

            var internalEvent = parsed.SelectToken(_internalEventNameJPathIdentifier);

            var parsedInternalEvent = internalEvent.ToObject(type);

            _dispatcher.RaiseEvent(parsedInternalEvent);

        }
    }
}
