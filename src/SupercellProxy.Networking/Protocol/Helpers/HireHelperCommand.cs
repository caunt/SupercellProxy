using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Hires one farm helper at a configured duration tier.</summary>
/// <param name="HelperIndex">Which helper to hire: 0 for the caretaker, 1 for the miller; written first as a VarInt.</param>
/// <param name="HireTierIndex">Zero-based index into the helper's <c>HireDays</c> and <c>DiamondPrice</c> columns, or -1 for the free first trial of <c>FirstTryHireDays</c>.</param>
public sealed record HireHelperCommand(int HelperIndex, int HireTierIndex) : CodecCommand<HireHelperCommand>, ICommandCodecProvider<HireHelperCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HireHelperCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.HelperIndex,
        Codecs.VarInt,
        static command => command.HireTierIndex,
        static (helperIndex, tierIndex) => new HireHelperCommand(helperIndex, tierIndex)));
}
