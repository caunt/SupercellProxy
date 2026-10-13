using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters.FreshBeats;

/// <summary>Records that the Fresh Beats screen was shown.</summary>
/// <param name="BeatIndex">The beat index.</param>
public sealed record MarkFreshBeatsSeenCommand(int BeatIndex) : CodecCommand<MarkFreshBeatsSeenCommand>, ICommandCodecProvider<MarkFreshBeatsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFreshBeatsSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.BeatIndex, static (beatIndex) => new MarkFreshBeatsSeenCommand(beatIndex)),
        CommandLayout.MetadataFirst
    );
}
