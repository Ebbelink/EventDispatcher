using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventEndpointDispatcher.Tests.TestData
{
    internal class Message
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("ts")]
        public string Timestamp { get; set; }
    }
}
