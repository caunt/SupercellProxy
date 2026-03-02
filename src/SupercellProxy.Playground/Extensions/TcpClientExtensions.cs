using System.Net.Sockets;

namespace SupercellProxy.Playground.Extensions;

public static class TcpClientExtensions
{
    extension(TcpClient tcpClient)
    {
        public string RemoteEndPoint => tcpClient.Client.RemoteEndPoint?.ToString() ?? throw new InvalidOperationException("RemoteEndPoint is null.");
    }
}
