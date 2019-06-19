public partial class StreamMessage
{
    public class WS
    {
        public static WSTestMessageDelegate OnWSTestMessage;

        public delegate void WSTestMessageDelegate(string topic, WSTestMessage message);
    }
}
