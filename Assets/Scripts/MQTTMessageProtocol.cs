using System;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

class MQTTMessageProtocol : IMessageProtocol
{
    MqttClient client { get; set; }
    string host;
    string channel;

    public MQTTMessageProtocol(string host, string channel)
    {
        this.host = host;
        this.channel = channel;
    }

    public string Name => throw new System.NotImplementedException();

    public bool Connect()
    {
        string clientId = Guid.NewGuid().ToString();

        client = new MqttClient(host);
        client.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) =>
        {
            string data = Encoding.UTF8.GetString(e.Message);
            string topic = e.Topic;
            var obj = JsonConvert.DeserializeObject<BaseStreamMessage>(data);
            obj.Dispatch(e.Topic);
        };
        client.MqttMsgSubscribed += (object sender, MqttMsgSubscribedEventArgs e) =>
        {
            Debug.Log($"{{ action: 'subscribe', {e.MessageId}, {e.GrantedQoSLevels[e.GrantedQoSLevels.Length - 1]} }}");
        };

        client.Connect(clientId);
        if (client.IsConnected)
        {
            client.Subscribe(new[] { "presence" }, new[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            client.Subscribe(new[] { channel }, new[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            return true;
        }
        return false;
    }

    public bool Publish(string text)
    {
        return client.Publish(channel, Encoding.UTF8.GetBytes(text), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false) == 1;
    }

    public void Dispose()
    {
        client.Disconnect();
    }
}