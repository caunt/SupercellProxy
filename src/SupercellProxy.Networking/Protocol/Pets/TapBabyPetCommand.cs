using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Makes an available Farm Pass baby pet run to another point near its house.</summary>
public sealed record TapBabyPetCommand(int PetGlobalId) : CodecCommand<TapBabyPetCommand>, ICommandCodecProvider<TapBabyPetCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TapBabyPetCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.PetGlobalId, static petGlobalId => new TapBabyPetCommand(petGlobalId)));
}
