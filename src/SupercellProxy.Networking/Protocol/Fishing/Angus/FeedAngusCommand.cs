using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Angus;

/// <summary>Gives goods to Angus.</summary>
/// <param name="GoodGlobalId">Global id of the goods given.</param>
/// <param name="AngusObjectGlobalId">Game object id of Angus at the fishing dock.</param>
/// <param name="Amount">How many of the goods are given.</param>
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
