using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Wakes a sleeping pet and collects its feeding reward.</summary>
public sealed record WakePetCommand(int PetGlobalId) : CodecCommand<WakePetCommand>, ICommandCodecProvider<WakePetCommand>
{
    /// <inheritdoc />
    public static CommandCodec<WakePetCommand> Codec { get; } = Codecs.Command(
        type: 87,
        Codecs.Composite(Codecs.VarInt, static command => command.PetGlobalId, static (petGlobalId) => new WakePetCommand(petGlobalId))
    );
}
