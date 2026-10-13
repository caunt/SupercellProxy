using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Hires Tom the boy with the chosen hire offer.</summary>
/// <param name="HireIndex">The chosen hire offer index.</param>
public sealed record HireBoyCommand(int HireIndex) : CodecCommand<HireBoyCommand>, ICommandCodecProvider<HireBoyCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HireBoyCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.HireIndex, static (hireIndex) => new HireBoyCommand(hireIndex)));
}
