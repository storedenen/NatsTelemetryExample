using System;

namespace NatsTelemetryExample.PubSub
{
    public class PubSubPayload
    {
        public object Value { get; set; }
        public DateTime SourceTimestamp { get; set; }
        public DateTime ServerTimestamp { get; set; }
    }
}