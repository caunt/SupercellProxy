using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Restores or drops edit-mode changes left from an interrupted session.</summary>
/// <param name="Recover">Whether the interrupted changes are restored.</param>
public sealed record RecoverLayoutChangesCommand(bool Recover) : CodecCommand<RecoverLayoutChangesCommand>, ICommandCodecProvider<RecoverLayoutChangesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecoverLayoutChangesCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.Recover, static (recover) => new RecoverLayoutChangesCommand(recover))
    );
}
