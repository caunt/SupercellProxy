using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Help;

/// <summary>Requests public, neighbourhood, or Farm Pass help for a boat crate.</summary>
/// <param name="CrateIndex">The zero-based index of the crate in the docked boat order's crate list, written as a UInt16.</param>
/// <param name="RequestKind">Who the request is addressed to, written as a UInt16: 1 friends, 2 the neighbourhood, 3 through the Farm Pass perk.</param>
public sealed record RequestBoatCrateHelpCommand(int CrateIndex, int RequestKind) : CodecCommand<RequestBoatCrateHelpCommand>, ICommandCodecProvider<RequestBoatCrateHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestBoatCrateHelpCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.UInt16,
        static command => ushort.CreateTruncating(command.CrateIndex),
        Codecs.UInt16,
        static command => ushort.CreateTruncating(command.RequestKind),
        static (crateIndex, requestKind) => new RequestBoatCrateHelpCommand(crateIndex, requestKind)));
}
