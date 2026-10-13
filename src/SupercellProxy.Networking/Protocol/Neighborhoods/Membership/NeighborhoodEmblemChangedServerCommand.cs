using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Replaces the emblem of the avatar's neighborhood.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="EmblemBackground">The first emblem part.</param>
/// <param name="EmblemPattern">The second emblem part.</param>
/// <param name="EmblemSymbol">The third emblem part.</param>
public sealed record NeighborhoodEmblemChangedServerCommand(LongId NeighborhoodId, int EmblemBackground, int EmblemPattern, int EmblemSymbol) : CodecServerCommand<NeighborhoodEmblemChangedServerCommand>, ICommandCodecProvider<NeighborhoodEmblemChangedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodEmblemChangedServerCommand> Codec { get; } = Codecs.Command(
        type: 136,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.VarInt,
            static command => command.EmblemBackground,
            Codecs.VarInt,
            static command => command.EmblemPattern,
            Codecs.VarInt,
            static command => command.EmblemSymbol,
            static (neighborhoodId, emblemBackground, emblemPattern, emblemSymbol) => new NeighborhoodEmblemChangedServerCommand(neighborhoodId, emblemBackground, emblemPattern, emblemSymbol)
        )
    );
}
