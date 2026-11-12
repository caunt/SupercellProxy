using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Wakes a sleeping pet and collects its feeding reward.</summary>
/// <param name="PetGlobalId">
/// Object id of the pet on the player's own farm, written as a VarInt.
/// </param>
public sealed record WakePetCommand(int PetGlobalId) : CodecCommand<WakePetCommand>, ICommandCodecProvider<WakePetCommand>
{
    /// <inheritdoc />
    public static CommandCodec<WakePetCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.PetGlobalId, static petGlobalId => new WakePetCommand(petGlobalId)));
}
