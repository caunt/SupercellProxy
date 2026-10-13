using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Angus;

/// <summary>Gives goods to Angus.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="AngusObjectGlobalId">The angus object id.</param>
/// <param name="Amount">The amount.</param>
public sealed record FeedAngusCommand(int GoodGlobalId, int AngusObjectGlobalId, int Amount) : CodecCommand<FeedAngusCommand>, ICommandCodecProvider<FeedAngusCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FeedAngusCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GoodGlobalId,
            Codecs.VarInt,
            static command => command.AngusObjectGlobalId,
            Codecs.VarInt,
            static command => command.Amount,
            static (goodGlobalId, angusObjectGlobalId, amount) => new FeedAngusCommand(goodGlobalId, angusObjectGlobalId, amount)
        ),
        CommandLayout.MetadataFirst
    );
}
