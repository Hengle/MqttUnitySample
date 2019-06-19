using System;

public interface IMessageProtocol : IDisposable
{
    string Name { get; }

    bool Connect();
    bool Publish(string text);
}
