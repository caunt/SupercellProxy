using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// Explains why the server is closing the current client session.
public sealed record DisconnectedMessage(DisconnectReason Reason) : IMessage
{
    /// Decodes the Titan disconnect reason carried as a fixed-width integer.
    public static DisconnectedMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        DisconnectedMessage message = new(System.Runtime.CompilerServices.Unsafe.BitCast<int, DisconnectReason>(stream.ReadInt32()));

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The disconnect notification has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<DisconnectReason, int>(Reason));
    }
}
