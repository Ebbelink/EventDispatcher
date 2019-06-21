using Microsoft.AspNetCore.Http;

namespace EventEndpointDispatcher
{
    /// <summary>
    /// Processor for events
    /// </summary>
    public interface IEventProcessor
    {
        /// <summary>
        /// Read the body from the <see cref="HttpContext"/> and extract the internal event from it.
        /// Parse that internal event to a concrete type and raise the callbacks for the event
        /// </summary>
        /// <param name="context"></param>
        void ProcessEvent(HttpContext context);
    }
}
