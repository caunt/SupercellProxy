using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>Claws back goods of one type from the avatar.</summary>
/// <param name="GoodGlobalId">The revoked good data id.</param>
/// <param name="Amount">The revoked amount.</param>
public sealed record RevokeGoodServerCommand(int GoodGlobalId, int Amount) : CodecServerCommand<RevokeGoodServerCommand>, ICommandCodecProvider<RevokeGoodServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RevokeGoodServerCommand> Codec { get; } = Codecs.Command(
        type: 248,
        Codecs.Composite(
            Codecs.Int32,
            static command => command.GoodGlobalId,
            Codecs.VarInt,
            static command => command.Amount,
            static (goodGlobalId, amount) => new RevokeGoodServerCommand(goodGlobalId, amount)
        ),
        CommandLayout.MetadataFirst
    );
}
