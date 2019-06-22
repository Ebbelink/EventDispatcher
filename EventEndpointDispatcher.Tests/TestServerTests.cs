using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace EventEndpointDispatcher.Tests
{
    public class TestServerTests
    {
        private string _baseJsonMessageEvent;

        public TestServerTests()
        {
            _baseJsonMessageEvent = "" +
                "{                                   \n" +
                "	\"type\": \"message\",           \n" +
                "	\"event\": {                     \n" +
                "	    \"type\": \"message\",       \n" +
                "	    \"user\": \"U2147483697\",   \n" +
                "	    \"text\": \"Hello world\",   \n" +
                "	    \"ts\": \"1355517523.000005\"\n" +
                "	}                                \n" +
                "}                                   \n";
        }

    }
}
