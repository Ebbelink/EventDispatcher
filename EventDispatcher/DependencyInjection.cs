using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        public static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        {
            EventDispatcher dispatcher = new EventDispatcher();

            services.AddSingleton<IEventDispatcher>(dispatcher);

            services.AddScoped<IEventProcessor, EventProcessor>();

            return services;
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
        public static IServiceCollection AddEventDispatcher(this IServiceCollection services, params Assembly[] eventsAssemblies)
        {
            EventDispatcher dispatcher = new EventDispatcher();

            foreach (var assembly in eventsAssemblies)
            {
                var eventTypeToConcreteMapping = GetConcreteEventModels(typeof(DependencyInjection).Assembly);

                foreach (var eventMap in eventTypeToConcreteMapping)
                {
                    dispatcher.RegisterEvent(eventMap.Key, eventMap.Value);
                }
            }

            services.AddSingleton<IEventDispatcher>(dispatcher);

            services.AddScoped<IEventProcessor, EventProcessor>();

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
