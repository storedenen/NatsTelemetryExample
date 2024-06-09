namespace NatsTelemetryExample.PubSub
{
    using System;
    
    public class PubSubPayloadValue
    {
        public string Id { get; set; }
        public object Value { get; set; }
        public DateTime SourceTimestamp { get; set; }
        public DateTime ServerTimestamp { get; set; }
    }
}