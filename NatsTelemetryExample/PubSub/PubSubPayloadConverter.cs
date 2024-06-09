namespace NatsTelemetryExample.PubSub
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    
    public class PubSubPayloadConverter : JsonConverter<PubSubPayload>
    {
        public override PubSubPayload? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var valueConverter = (JsonConverter<PubSubPayloadValue>)options.GetConverter(typeof(PubSubPayloadValue));

            var result = new PubSubPayload();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            
            var hasMessage = true;
            result.PayloadValues = new List<PubSubPayloadValue>();

            while (hasMessage)
            {
                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Invalid TokenType {reader.TokenType}");
                }

                var payloadId = reader.GetString();

                reader.Read();

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }
                
                var newPayloadValue = valueConverter.Read(ref reader, typeof(PubSubPayloadValue), options);

                newPayloadValue.Id = payloadId;
                result.PayloadValues.Add(newPayloadValue);

                reader.Read();

                hasMessage = reader.TokenType != JsonTokenType.EndObject;
            }
            
            return result;
        }

        public override void Write(Utf8JsonWriter writer, PubSubPayload value, JsonSerializerOptions options)
        {
        }
    }
}