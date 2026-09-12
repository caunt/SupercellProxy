using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// Explains why the server is closing the current client session.
public sealed record DisconnectedMessage(DisconnectReason Reason) : IMessage
{
    /// Decodes the Titan disconnect reason carried as a fixed-width integer.
    public static DisconnectedMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        DisconnectedMessage message = new(System.Runtime.CompilerServices.Unsafe.BitCast<int, DisconnectReason>(container.Payload.ReadInt32()));

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The disconnect notification has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<DisconnectReason, int>(Reason));

        return new MessageContainer(identifier, version, stream);
    }
}
