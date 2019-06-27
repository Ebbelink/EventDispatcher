using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Threading.Tasks;

namespace EventEndpointDispatcher
{
    /// <summary>
    /// Event processing middleware
    /// This is being used to dynamically set the path of the event 'endpoint'
    /// </summary>
    public sealed class EventMiddleware
    {
        /// <summary>
        /// Constructor for the <see cref="EventMiddleware"/>
        /// Used for dependency injection
        /// </summary>
        /// <param name="next">the delegate to execute next</param>
        public EventMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        private readonly RequestDelegate _next;

        /// <summary>
        /// default Middleware implementation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="eventProcessor">the event processor used to process generic events</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IEventProcessor eventProcessor)
        {
            // Make the request replayable
            context.Request.EnableRewind();

            // The handle url is remapped to the root dir in the UseEventEndpoint call. No matter what the user gives as input
            // So if the request is not going to the root directory it is not meant for us
            if (!string.IsNullOrWhiteSpace(context.Request.Path.Value.Trim('/')))
            {
                await _next(context);
                return;
            }

            if (eventProcessor == null)
                throw new ArgumentNullException(nameof(eventProcessor), $"It is most likely you did not register {nameof(IEventProcessor)}");

            var request = context.Request;
            var response = context.Response;

            response.StatusCode = StatusCodes.Status200OK;

            // Do the event processing logic here
            eventProcessor.ProcessEvent(context);
        }
    }
}
