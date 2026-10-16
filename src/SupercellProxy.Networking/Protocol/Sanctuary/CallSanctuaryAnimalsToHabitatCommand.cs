using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Calls compatible sanctuary animals home and collects rewards from animals woken by the call.</summary>
public sealed record CallSanctuaryAnimalsToHabitatCommand(int HabitatGlobalId) : CodecCommand<CallSanctuaryAnimalsToHabitatCommand>, ICommandCodecProvider<CallSanctuaryAnimalsToHabitatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CallSanctuaryAnimalsToHabitatCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            static habitatGlobalId => new CallSanctuaryAnimalsToHabitatCommand(habitatGlobalId)
        )
    );
}
