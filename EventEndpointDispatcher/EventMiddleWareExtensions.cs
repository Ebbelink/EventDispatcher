using Microsoft.AspNetCore.Builder;
using System;

namespace EventEndpointDispatcher
{
    /// <summary>
    /// Extension methods for the <see cref="EventMiddleware"/> pipeline to run
    /// </summary>
    public static class EventMiddleWareExtensions
    {
        /// <summary>
        /// Start up an events endpoint to dispatch events to the registered callbacks via <see cref="IEventDispatcher.RegisterCallback{EventType}(Action{EventType})"/>.
        /// In order to use this <see cref="DependencyInjection.AddEventDispatcher(Microsoft.Extensions.DependencyInjection.IServiceCollection)"/> has be used to register dependencies
        /// </summary>
        public static IApplicationBuilder UseEventsEndpoint(this IApplicationBuilder builder, string url = "/Events")
        {
            // If there is a url to map map it to root 
            if (url != null)
                return builder.Map(url, (builder2) =>
                {
                    builder2.UseEventsEndpoint(null);
                });

            return builder.UseMiddleware<EventMiddleware>();
        }
    }
}
