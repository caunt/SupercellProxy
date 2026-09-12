using System.Net.Sockets;

namespace SupercellProxy.Networking.Transport;

/// <summary>
/// Represents <c language="csharp">TcpClientExtensions</c>.
/// </summary>
public static class SocketClientExtensions
{
    /// <summary>
    /// <para>Returns the connected remote endpoint.</para>
    /// </summary>
    public static string GetRemoteEndPoint(this TcpClient socketClient)
    {
        ArgumentNullException.ThrowIfNull(socketClient);

        return socketClient.Client.RemoteEndPoint?.ToString()
            ?? throw new InvalidOperationException(message: "RemoteEndPoint is null.");
    }
}
