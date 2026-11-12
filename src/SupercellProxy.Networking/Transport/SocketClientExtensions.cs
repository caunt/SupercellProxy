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
    /// <param name="socketClient">A connected TCP client, such as a game client accepted by the proxy.</param>
    /// <returns>The peer's address and port as text, such as <c>192.168.1.5:51234</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="socketClient"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The socket has no remote endpoint because it is not connected.</exception>
    public static string GetRemoteEndPoint(this TcpClient socketClient)
    {
        ArgumentNullException.ThrowIfNull(socketClient);

        return socketClient.Client.RemoteEndPoint?.ToString()
            ?? throw new InvalidOperationException(message: "RemoteEndPoint is null.");
    }
}
