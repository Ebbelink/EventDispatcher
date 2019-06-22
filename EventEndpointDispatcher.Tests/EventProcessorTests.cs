using EventEndpointDispatcher;
using EventEndpointDispatcher.Tests.TestData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace EventEndpointDispatcher.Tests
{
    public class EventProcessorTests
    {
        private const string _messageEventJson = @"
            {
	            ""type"": ""message"",
	            ""event"": {
	                ""type"": ""message"",
	                ""user"": ""mad@madailei.com"",
	                ""text"": ""Hello world"",
	                ""ts"": ""1355517523.000005""
	            }
            }";

        private IEventProcessor _processor;
        private IEventDispatcher _dispatcher;

        public EventProcessorTests()
        {
            _dispatcher = new EventDispatcher();
            _dispatcher.RegisterEvent("message", typeof(Message));

            _processor = new EventProcessor(_dispatcher, "type", "event");
        }

        [Fact]
        public void EventProcessorTest_Message()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(_messageEventJson));

            _dispatcher.RegisterCallback<Message>((message) =>
            {
                Assert.Equal("mad@madailei.com", message.User);
                Assert.Equal("Hello world", message.Text);
                Assert.Equal("1355517523.000005", message.Timestamp);
            });

            _processor.ProcessEvent(context);
        }
    }
}
