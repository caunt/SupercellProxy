using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Opens a Tiny Trail chest.</summary>
/// <param name="UseDiamonds">The use diamonds.</param>
public sealed record OpenMiniPassChestCommand(bool UseDiamonds) : CodecCommand<OpenMiniPassChestCommand>, ICommandCodecProvider<OpenMiniPassChestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenMiniPassChestCommand> Codec { get; } = Codecs.Command(
        type: 591,
        Codecs.Composite(Codecs.Boolean, static command => command.UseDiamonds, static (useDiamonds) => new OpenMiniPassChestCommand(useDiamonds)),
        CommandLayout.MetadataFirst
    );
}
