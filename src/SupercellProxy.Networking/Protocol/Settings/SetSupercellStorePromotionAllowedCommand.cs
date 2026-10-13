using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Allows or forbids Supercell Store promotions.</summary>
/// <param name="Allowed">Whether Supercell Store promotions are allowed.</param>
public sealed record SetSupercellStorePromotionAllowedCommand(bool Allowed) : CodecCommand<SetSupercellStorePromotionAllowedCommand>, ICommandCodecProvider<SetSupercellStorePromotionAllowedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetSupercellStorePromotionAllowedCommand> Codec { get; } = Codecs.Command(
        type: 585,
        Codecs.Composite(Codecs.Boolean, static command => command.Allowed, static (allowed) => new SetSupercellStorePromotionAllowedCommand(allowed)),
        CommandLayout.MetadataFirst
    );
}
