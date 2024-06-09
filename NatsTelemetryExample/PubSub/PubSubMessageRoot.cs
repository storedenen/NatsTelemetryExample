namespace NatsTelemetryExample.PubSub
{
    public class PubSubMessageRoot
    {
        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string PublisherId { get; set; }
        public string DataSetWriterGroup { get; set; }
        public PubSubMessage[] Messages { get; set; }
    }
}


