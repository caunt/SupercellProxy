using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Taps a registered ambient animal to resume its movement.</summary>
/// <param name="AmbientAnimalGlobalId">Game object id of the ambient animal on the farm that was tapped, sent as a VarInt.</param>
public sealed record TapAmbientAnimalCommand(int AmbientAnimalGlobalId) : CodecCommand<TapAmbientAnimalCommand>, ICommandCodecProvider<TapAmbientAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TapAmbientAnimalCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.AmbientAnimalGlobalId, static id => new TapAmbientAnimalCommand(id)));
}
