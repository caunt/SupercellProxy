using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Defines the Home Load Failed Message contract.
/// </summary>
/// <summary>
/// Defines the Reason contract.
/// </summary>
public sealed record HomeLoadFailedMessage(HomeVisitFailureReason Reason) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static HomeLoadFailedMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        HomeLoadFailedMessage result = new(System.Runtime.CompilerServices.Unsafe.BitCast<int, HomeVisitFailureReason>(container.Payload.ReadVariableInt()));

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Home-load failure has trailing data.")
            : result;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(System.Runtime.CompilerServices.Unsafe.BitCast<HomeVisitFailureReason, int>(Reason));

        return new MessageContainer(identifier, version, stream);
    }
}
