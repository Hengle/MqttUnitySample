public partial class StreamMessage
{
    public class MQTT
    {
        public static ClanJoinMessageDelegate OnClanJoinMessage;
        public static ServerMessageDelegate OnServerMessage;

        public delegate void ClanJoinMessageDelegate(string topic, ClanJoinMessage message);
        public delegate void ServerMessageDelegate(string topic, ServerMessage message);
    }
}
