using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Hires one farm helper at a configured duration tier.</summary>
public sealed record HireHelperCommand(int HelperIndex, int HireTierIndex) : CodecCommand<HireHelperCommand>, ICommandCodecProvider<HireHelperCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HireHelperCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HelperIndex,
            Codecs.VarInt,
            static command => command.HireTierIndex,
            static (helperIndex, tierIndex) => new HireHelperCommand(helperIndex, tierIndex)
        )
    );
}
