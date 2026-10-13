using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Requests public, neighbourhood, or Farm Pass help for a boat crate.</summary>
public sealed record RequestBoatCrateHelpCommand(int CrateIndex, int RequestKind) : CodecCommand<RequestBoatCrateHelpCommand>, ICommandCodecProvider<RequestBoatCrateHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestBoatCrateHelpCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.UInt16,
            static command => (ushort)command.CrateIndex,
            Codecs.UInt16,
            static command => (ushort)command.RequestKind,
            static (crateIndex, requestKind) => new RequestBoatCrateHelpCommand(crateIndex, requestKind)
        )
    );
}
