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
    public static HomeLoadFailedMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        HomeLoadFailedMessage result = new(System.Runtime.CompilerServices.Unsafe.BitCast<int, HomeVisitFailureReason>(stream.ReadVarInt()));

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Home-load failure has trailing data.")
            : result;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(System.Runtime.CompilerServices.Unsafe.BitCast<HomeVisitFailureReason, int>(Reason));
    }
}
