using System.Text.Json.Serialization;

namespace NatsTelemetryExample.PubSub
{
    using System.Collections.Generic;

    [JsonConverter(typeof(PubSubPayloadConverter))]
    public class PubSubPayload
    {
        public List<PubSubPayloadValue> PayloadValues { get; set; }
    }
}