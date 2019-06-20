using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventDispatcher
{
    /// <summary>
    /// Event dispatcher Dependency Injection Extension methods
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Setup the event dispatcher<br/>
        /// Concrete event types are registered by decorating them with the <see cref="EventTypeAttribute"/>
        /// Registers:<br/>
        /// <see cref="IEventDispatcher"/><br/>
        /// <see cref="IEventProcessor"/><br/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> service collection framework to register dependencies</param>
        /// <param name="eventTypeJPathIdentifier">A JPath identiefier identifying where to find the event type for the events. Used for mapping to the concrete types</param>
        /// <param name="internalEventNameJPathIdentifier">A JPath identiefier identifying where to find the internal event. Used as input for the concrete event</param>
        public static IServiceCollection AddEventDispatcher(this IServiceCollection services, string eventTypeJPathIdentifier = "type", string internalEventNameJPathIdentifier = "event")
        {
            return AddEventDispatcher(services, eventTypeJPathIdentifier, internalEventNameJPathIdentifier, null);
        }

        /// <summary>
        /// Setup the event dispatcher<br/>
        /// Concrete event types are registered by decorating them with the <see cref="EventTypeAttribute"/>
        /// Registers:<br/>
        /// <see cref="IEventDispatcher"/><br/>
        /// <see cref="IEventProcessor"/><br/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> service collection framework to register dependencies</param>
        /// <param name="eventsAssemblies">The assemblies to look in for events decorated with the <see cref="EventTypeAttribute"/> that need to be registered</param>
        /// <param name="eventTypeJPathIdentifier">A JPath identiefier identifying where to find the event type for the events. Used for mapping to the concrete types</param>
        /// <param name="internalEventNameJPathIdentifier">A JPath identiefier identifying where to find the internal event. Used as input for the concrete event</param>
        public static IServiceCollection AddEventDispatcher(this IServiceCollection services, string eventTypeJPathIdentifier = "type", string internalEventNameJPathIdentifier = "event", params Assembly[] eventsAssemblies)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrEmpty(internalEventNameJPathIdentifier))
                throw new ArgumentNullException(nameof(internalEventNameJPathIdentifier));
            if (string.IsNullOrEmpty(eventTypeJPathIdentifier))
                throw new ArgumentNullException(nameof(eventTypeJPathIdentifier));

            EventDispatcher dispatcher = new EventDispatcher();

            if (eventsAssemblies != null)
            {
                foreach (var assembly in eventsAssemblies)
                {
                    var eventTypeToConcreteMapping = GetConcreteEventModels(assembly);

                    foreach (var eventMap in eventTypeToConcreteMapping)
                    {
                        dispatcher.RegisterEvent(eventMap.Key, eventMap.Value);
                    }
                }
            }

            services.AddSingleton<IEventDispatcher>(dispatcher);

            services.AddScoped<IEventProcessor>(s => new EventProcessor(dispatcher, eventTypeJPathIdentifier, internalEventNameJPathIdentifier));

            return services;
        }

        private static IDictionary<string, Type> GetConcreteEventModels(Assembly assembly)
        {
            Dictionary<string, Type> eventTypeToConcreteMapping = new Dictionary<string, Type>();

            foreach (Type type in assembly.GetTypes())
            {
                Attribute foundAttribute = type.GetCustomAttribute(typeof(EventTypeAttribute), true);

                if (foundAttribute != null)
                {
                    string eventName = foundAttribute.GetType()
                        .GetProperty(nameof(EventTypeAttribute.EventName))
                            .GetValue(foundAttribute) as string;

                    eventTypeToConcreteMapping.Add(eventName, type);
                }
            }

            return eventTypeToConcreteMapping;
        }

    }
}
