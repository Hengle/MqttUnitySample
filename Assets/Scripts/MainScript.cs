using System;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using System.Net.Http;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    StreamMessage streamMessage { get; set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void chatJoin()
    {
        var auth = new ChatAuth().Register();
        var messageProtocol = new MQTTMessageProtocol(auth.host, auth.public_ch);
        streamMessage = new StreamMessage(messageProtocol);
        
        StreamMessage.MQTT.OnClanJoinMessage += (string topic, ClanJoinMessage msg) =>
        {
            Debug.Log($"ClanJoinMessage : {topic} {msg.ty} {msg.type} {msg.message}");
        };

        StreamMessage.MQTT.OnServerMessage += (string topic, ServerMessage msg) =>
        {
            Debug.Log($"ServerMessage : {topic} {msg.ty} {msg.type} {msg.message}");
        };

        streamMessage.Connect();
    }
    public void chatPublish()
    {
        var strings = new string[]{
            "{type: 'ClanJoin', ty: 1, message: 'clanjoined'}",
            "{type: 'Server', ty: 2, message: 'server-message'}",
        };
        foreach (var str in strings)
        {
            streamMessage.Publish(str);
        }
    }
    
    public void wsConnect()
    {
        var messageProtocol = new WSMessageProtocol("ws://127.0.0.1:40510/");
        streamMessage = new StreamMessage(messageProtocol);
        
        StreamMessage.WS.OnWSTestMessage += (string topic, WSTestMessage msg) =>
        {
            Debug.Log($"WSTestMessage : {topic} {msg.ty} {msg.type} {msg.message}");
        };

        if (!streamMessage.Connect())
        {
            Debug.Log("Connect failed");
        }
    }

}
