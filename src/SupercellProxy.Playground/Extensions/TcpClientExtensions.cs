using System.Net.Sockets;

namespace SupercellProxy.Playground.Extensions;

/// <summary>
/// Represents <c>TcpClientExtensions</c>.
/// </summary>
public static class TcpClientExtensions
{
    /// <summary>
    /// <para>Returns the connected remote endpoint.</para>
    /// </summary>
    public static string GetRemoteEndPoint(this TcpClient tcpClient)
    {
        return tcpClient.Client.RemoteEndPoint?.ToString()
            ?? throw new InvalidOperationException("RemoteEndPoint is null.");
    }
}
