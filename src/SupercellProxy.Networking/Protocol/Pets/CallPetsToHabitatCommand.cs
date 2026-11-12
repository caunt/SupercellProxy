using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Calls compatible pets to a habitat and collects rewards from pets woken by the call.</summary>
/// <param name="HabitatGlobalId">
/// Object id of the pet house on the player's own farm, written as a VarInt and the command's only field.
/// </param>
public sealed record CallPetsToHabitatCommand(int HabitatGlobalId) : CodecCommand<CallPetsToHabitatCommand>, ICommandCodecProvider<CallPetsToHabitatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CallPetsToHabitatCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.HabitatGlobalId,
        static habitatGlobalId => new CallPetsToHabitatCommand(habitatGlobalId)));
}
