using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Calls compatible pets to a habitat and collects rewards from pets woken by the call.</summary>
public sealed record CallPetsToHabitatCommand(int HabitatGlobalId) : CodecCommand<CallPetsToHabitatCommand>, ICommandCodecProvider<CallPetsToHabitatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CallPetsToHabitatCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            static (habitatGlobalId) => new CallPetsToHabitatCommand(habitatGlobalId)
        )
    );
}
