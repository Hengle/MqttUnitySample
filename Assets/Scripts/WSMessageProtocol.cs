using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

using Fleck;

class WSMessageProtocol : IMessageProtocol
{
    ClientWebSocket client { get; set; }
    string host;

    public WSMessageProtocol(string host)
    {
        this.host = host;
    }

    // Func<MessageEventArgs, Task> onMessage = e =>
    // {
    //     var data = e.Text.ReadToEnd();
    //     Debug.Log('a');
        
    //     var obj = JsonConvert.DeserializeObject<BaseStreamMessage>(data);
    //     Debug.Log($"{obj.ty} {obj.type} {obj.message}");
    //     //obj.Dispatch("WS");
    //     return Task.FromResult(false);
    // };

    public string Name => name;
    string name = "WSMessageProtocol";

    public bool Connect()
    {
        Uri u = new Uri("ws://127.0.0.1:40510");
        ClientWebSocket cws = null;
        ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);

        void Start() { Connect(); }

        async void Connect() {
            cws = new ClientWebSocket();
            try {
                await cws.ConnectAsync(u, CancellationToken.None);
                if (cws.State == WebSocketState.Open) Debug.Log("connected");
                SayHello();
                GetStuff();
            }
            catch (Exception e) { Debug.Log("woe " + e.Message); }
        }

        async void SayHello() {
            ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello"));
            await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        async void GetStuff() {
            WebSocketReceiveResult r = await cws.ReceiveAsync(buf, CancellationToken.None);
            var text = Encoding.UTF8.GetString(buf.Array, 0, r.Count);
            
            var obj = JsonConvert.DeserializeObject<BaseStreamMessage>(text);
            
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                obj.Dispatch("ws");
            });
            GetStuff();
        }
        Start();
        return true;
        // using (var ws = new WebSocket ("ws://dragonsnest.far/Laputa")) {
        // ws. += (sender, e) =>
        //   Console.WriteLine ("Laputa says: " + e.Data);

        // HttpListener httpListener = new HttpListener();
        // httpListener.Prefixes.Add("http://localhost/");
        // httpListener.Start();

        // HttpListenerContext context = httpListener.GetContextAsync().Result;
        // if (context.Request.IsWebSocketRequest)
        // {
        //     HttpListenerWebSocketContext webSocketContext = context.AcceptWebSocketAsync(null).Result;
        //     WebSocket webSocket = webSocketContext.WebSocket;
        //     while (webSocket.State == WebSocketState.Open)
        //     {
        //         await webSocket.SendAsync( ... );
        //     }
        // }
        
        // string clientId = Guid.NewGuid().ToString();

        // client = new MqttClient(host);
        // client.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) =>
        // {
        //     string data = Encoding.UTF8.GetString(e.Message);
        //     string topic = e.Topic;
        //     var obj = JsonConvert.DeserializeObject<BaseStreamMessage>(data);
        //     obj.Dispatch(e.Topic);
        // };
        // client.MqttMsgSubscribed += (object sender, MqttMsgSubscribedEventArgs e) =>
        // {
        //     Debug.Log($"{{ action: 'subscribe', {e.MessageId}, {e.GrantedQoSLevels[e.GrantedQoSLevels.Length - 1]} }}");
        // };

        // client.Connect(clientId);
        // if (client.IsConnected)
        // {
        //     client.Subscribe(new[] { "presence" }, new[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        //     client.Subscribe(new[] { channel }, new[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        //     return true;
        // }
        // return false;
    }

    public bool Publish(string text)
    {
        return true;
        //return client.Publish("channel", Encoding.UTF8.GetBytes(text), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false) == 1;
    }

    public void Dispose()
    {
        Debug.Log("dispose");
        client.Dispose();
    }
}