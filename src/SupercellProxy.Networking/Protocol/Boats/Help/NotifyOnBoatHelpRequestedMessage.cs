using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Help;

/// <summary>Tells visitors of a farm that its owner asked for help with a boat crate.</summary>
/// <param name="CrateIndex">Position of the crate the owner asked help with.</param>
/// <param name="OwnerId">The farm owner; the client ignores the message unless it is visiting that farm.</param>
/// <param name="HelperId">Player the request concerns, as the server reports it.</param>
public sealed record NotifyOnBoatHelpRequestedMessage(int CrateIndex, LongId OwnerId, LongId HelperId) : CodecMessage<NotifyOnBoatHelpRequestedMessage>, ICodecProvider<NotifyOnBoatHelpRequestedMessage>
{
    /// <inheritdoc />
    public static ICodec<NotifyOnBoatHelpRequestedMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.CrateIndex,
        Codecs.LongId,
        static message => message.OwnerId,
        Codecs.LongId,
        static message => message.HelperId,
        static (crateIndex, ownerId, helperId) => new NotifyOnBoatHelpRequestedMessage(crateIndex, ownerId, helperId)
    );
}
