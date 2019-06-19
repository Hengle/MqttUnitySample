using System;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;

[JsonConverter(typeof(StreamMessageConverter))]
public class BaseStreamMessage
{
    public int ty;
    public string type;
    public string message;

    public virtual void Dispatch(string topic)
    {
    }
}

public partial class StreamMessage
{
    MqttClient client { get; set; }
    IMessageProtocol MessageProtocol;

    public StreamMessage(IMessageProtocol MessageProtocol)
    {
        this.MessageProtocol = MessageProtocol ?? throw new ArgumentNullException(nameof(MessageProtocol));
    }
    
    public bool Connect()
    {
        return MessageProtocol.Connect();
    }

    public bool Publish(string text)
    {
        return MessageProtocol.Publish(text);
    }
}