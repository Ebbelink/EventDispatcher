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
            Console.WriteLine("start reading request body stream");
            StreamReader sr = new StreamReader(context.Request.Body);

            context.Request.Body.Position = 0;

            string bodyContents = sr.ReadToEnd();

            sr.Dispose();

            Console.WriteLine("parse the read body contents");
            var parsed = JObject.Parse(bodyContents);

            Console.WriteLine("get the event type node from the json");
            var eventType = parsed.SelectToken(_eventTypeJPathIdentifier);
            Console.WriteLine($"found event type: {eventType}");

            Console.WriteLine($"Retrieving the registered event type from the event dispatcher");
            Type type = _dispatcher.GetEventType(eventType.Value<string>());
            Console.WriteLine($"Retrieved event type is: {type.ToString()}");

            Console.WriteLine("parsing internal event");
            var internalEvent = parsed.SelectToken(_internalEventNameJPathIdentifier);

            Console.WriteLine("getting json of internal event");
            var parsedInternalEvent = internalEvent.ToObject(type);

            Console.WriteLine("Raising callbacks for event");
            _dispatcher.RaiseEvent(parsedInternalEvent);

        }
    }
}
