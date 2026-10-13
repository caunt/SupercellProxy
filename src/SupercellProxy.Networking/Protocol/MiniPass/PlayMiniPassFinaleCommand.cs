using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Plays the Tiny Trail finale.</summary>
/// <param name="MiniPassId">The mini pass id.</param>
public sealed record PlayMiniPassFinaleCommand(int MiniPassId) : CodecCommand<PlayMiniPassFinaleCommand>, ICommandCodecProvider<PlayMiniPassFinaleCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlayMiniPassFinaleCommand> Codec { get; } = Codecs.Command(
        type: 512,
        Codecs.Composite(Codecs.VarInt, static command => command.MiniPassId, static (miniPassId) => new PlayMiniPassFinaleCommand(miniPassId)),
        CommandLayout.MetadataFirst
    );
}
