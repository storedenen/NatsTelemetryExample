using System;

namespace NatsTelemetryExample.PubSub
{
    public class PubSubMessage
    {
        public PubSubMetaDataVersion MetaDataVersion { get; set; }
        public string MessageType { get; set; }
        public DateTime Timestamp { get; set; }
        public PubSubPayload[] Payloads { get; set; }
    }
}