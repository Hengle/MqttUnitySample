using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class StreamMessageConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken jObject = JToken.ReadFrom(reader);
        StreamMessageType type = jObject["type"].ToObject<StreamMessageType>();

        BaseStreamMessage msg;
        switch(type)
        {
            case StreamMessageType.ClanJoin:
            {
                msg = new ClanJoinMessage();
                break;
            }

            case StreamMessageType.Server:
            {
                msg = new ServerMessage();
                break;
            }

            case StreamMessageType.WSTest:
            {
                msg = new WSTestMessage();
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        serializer.Populate(jObject.CreateReader(), msg);
        return msg;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
