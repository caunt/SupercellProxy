using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Plays the Tiny Trail finale up to the given points.</summary>
/// <param name="SeenPoints">Points the finale played up to, saved as the current pass's seen points.</param>
public sealed record PlayMiniPassFinaleCommand(int SeenPoints) : CodecCommand<PlayMiniPassFinaleCommand>, ICommandCodecProvider<PlayMiniPassFinaleCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlayMiniPassFinaleCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SeenPoints, static seenPoints => new PlayMiniPassFinaleCommand(seenPoints)),
        CommandLayout.MetadataFirst
    );
}
