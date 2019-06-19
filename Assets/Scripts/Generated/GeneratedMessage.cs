public enum StreamMessageType
{
    Unknown,
    ClanJoin,
    Server,
    WSTest
}

public class ClanJoinMessage : BaseStreamMessage
{
    public override void Dispatch(string topic)
    {
        StreamMessage.MQTT.OnClanJoinMessage?.Invoke(topic, this);
    }
}

public class ServerMessage : BaseStreamMessage
{
    public override void Dispatch(string topic)
    {
        StreamMessage.MQTT.OnServerMessage?.Invoke(topic, this);
    }
}

public class WSTestMessage : BaseStreamMessage
{
    public override void Dispatch(string topic)
    {
        StreamMessage.WS.OnWSTestMessage?.Invoke(topic, this);
    }
}