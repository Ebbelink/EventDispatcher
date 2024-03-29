## Lightweight, easy to use, generic event endpoint and dispatcher

The event dispatcher endpoint is a generic event endpoint that can be plugged into the middleware of your application. It will spin up an endpoint to which events can be send and it will dispatch these events to the registered callbacks

## How to setup

### Install EventEndpointDispatcher from NuGet

`Install-Package EventEndpointDispatcher`

### Modify the Startup.cs

#### Add the using
`using EventEndpointDispatcher;`

#### Add the event dispatcher 
`services.AddEventDispatcher("type", "event", typeof(Startup).Assembly);`
The AddEventDispatchers signature is the following:
```
public static IServiceCollection AddEventDispatcher(this IServiceCollection services, string eventTypeJPathIdentifier = "type", string internalEventNameJPathIdentifier = "event");
public static IServiceCollection AddEventDispatcher(this IServiceCollection services, string eventTypeJPathIdentifier = "type", string internalEventNameJPathIdentifier = "event", params Assembly[] eventsAssemblies);
```
There is a base which takes a JPath path query to find the type of the event and one to find the internal event object. The type of the event is used to route the event to the correct event handler. The internal event is dispatched to the registered callback for the event type.

If you don't want to manually register every event data class you can use the overload in which an array of assemblies can be passed. The AddEventDispatcher will look for classes that are decorated with the `[EventType("EVENT_TYPE_NAME")]` attribute and automatically register these as the concrete implementation for the given event type.

#### Register the handler you want to use
`services.AddTransient<EventsHandler>();`
This event handler will be retrieved later in the startup to register callbacks on

More on how the handler needs to look like in [Create the handler](#Create-the-handler)

#### Use the event endpoint
The steps are:
1. spin up the endpoint
2. retrieve the instance of the EventsHandler in order to register the callbacks
3. retrieve the IEventDispatcher in order to register the callbacks
4. call dispatcher.RegisterCallback with a Type parameter for the EventType (Event in this case) to register a callback for. The callback is found in the EventHandler class as seen above.
```
// 1. spin up the endpoint
app.UseEventsEndpoint("/events");

// 2. retrieve the instance of the EventsHandler in order to register the callbacks
EventsHandler eventHandler = app.ApplicationServices.GetService<EventsHandler>(); 

// 3. retrieve the IEventDispatcher in order to register the callbacks
IEventDispatcher dispatcher = app.ApplicationServices.GetService<IEventDispatcher>();
// 4. call dispatcher.RegisterCallback with a Type parameter for the EventType (Event in this case) to register a callback for.
// The callback is found in the EventHandler class as seen above.
dispatcher.RegisterCallback<CarDelivered>(eventHandler.SomeEventsHandler);
```

### Create the handler

The handler will need to look like this:
```
public class EventsHandler
{
    public void SomeEventsHandler(Event someEvent)
    {
        // Do something with someEvent here...
    }
}
```

the only thing that matter here is that the function needs to be accessible from the startup. You can fix this by doing a GetService on the ApplicationServices if you need dependency injection in the event handler
