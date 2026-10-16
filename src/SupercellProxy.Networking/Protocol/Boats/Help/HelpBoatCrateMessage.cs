using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Help;

/// <summary>Fills a boat crate of the visited farm whose owner asked for help with it.</summary>
/// <param name="UseDiamonds">Whether diamonds pay for the goods the helper lacks.</param>
/// <param name="ProductGlobalId">The crate's product.</param>
/// <param name="Amount">The crate's amount.</param>
/// <param name="CrateIndex">Position of the crate in the owner's boat order; the owner must have asked for help with it.</param>
public sealed record HelpBoatCrateMessage(bool UseDiamonds, int ProductGlobalId, int Amount, int CrateIndex) : CodecMessage<HelpBoatCrateMessage>, ICodecProvider<HelpBoatCrateMessage>
{
    /// <inheritdoc />
    public static ICodec<HelpBoatCrateMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.UseDiamonds,
        Codecs.VarInt,
        static message => message.ProductGlobalId,
        Codecs.VarInt,
        static message => message.Amount,
        Codecs.VarInt,
        static message => message.CrateIndex,
        static (useDiamonds, productGlobalId, amount, crateIndex) => new HelpBoatCrateMessage(useDiamonds, productGlobalId, amount, crateIndex)
    );
}
