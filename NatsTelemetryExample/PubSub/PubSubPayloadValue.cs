namespace NatsTelemetryExample.PubSub
{
    using System;
    
    public class PubSubPayloadValue
    {
        public string Id { get; set; }
        public object Value { get; set; }
        public DateTimeOffset SourceTimestamp { get; set; }
        public DateTimeOffset ServerTimestamp { get; set; }
    }
}