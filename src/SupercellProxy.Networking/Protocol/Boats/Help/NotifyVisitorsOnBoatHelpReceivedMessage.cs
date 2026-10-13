using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Help;

/// <summary>Tells visitors of a farm that a boat crate received help, so the crate shows its helper.</summary>
/// <param name="CrateIndex">The crate index.</param>
/// <param name="HelperId">The helper id.</param>
/// <param name="OwnerId">The farm owner; the client ignores the message unless it is visiting that farm.</param>
public sealed record NotifyVisitorsOnBoatHelpReceivedMessage(int CrateIndex, LongId HelperId, LongId OwnerId) : CodecMessage<NotifyVisitorsOnBoatHelpReceivedMessage>, ICodecProvider<NotifyVisitorsOnBoatHelpReceivedMessage>
{
    /// <inheritdoc />
    public static ICodec<NotifyVisitorsOnBoatHelpReceivedMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.CrateIndex,
        Codecs.LongId,
        static message => message.HelperId,
        Codecs.LongId,
        static message => message.OwnerId,
        static (crateIndex, helperId, ownerId) => new NotifyVisitorsOnBoatHelpReceivedMessage(crateIndex, helperId, ownerId)
    );
}
