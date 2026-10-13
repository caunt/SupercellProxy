using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Makes an awake pet without an active care movement run around its habitat.</summary>
public sealed record TapPetCommand(int PetGlobalId) : CodecCommand<TapPetCommand>, ICommandCodecProvider<TapPetCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TapPetCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.PetGlobalId, static (petGlobalId) => new TapPetCommand(petGlobalId)));
}
