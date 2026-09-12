using System.Net.Sockets;

using SupercellProxy.Networking.Cryptography;

namespace SupercellProxy.Networking.Proxy;

internal static class ProxyFailureClassifier
{
    internal static bool IsRecoverable(Exception exception)
    {
        return exception is IOException or SocketException or InvalidDataException
            or OperationCanceledException or TimeoutException or NaClV3Exception;
    }
}
