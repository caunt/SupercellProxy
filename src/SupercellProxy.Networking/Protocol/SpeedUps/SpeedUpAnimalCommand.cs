using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SpeedUps;

/// <summary>Finishes an animal's production immediately for diamonds.</summary>
/// <param name="AnimalGlobalId">The animal game object id.</param>
public sealed record SpeedUpAnimalCommand(int AnimalGlobalId) : CodecCommand<SpeedUpAnimalCommand>, ICommandCodecProvider<SpeedUpAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpAnimalCommand> Codec { get; } = Codecs.Command(
        type: 15,
        Codecs.Composite(Codecs.VarInt, static command => command.AnimalGlobalId, static (animalGlobalId) => new SpeedUpAnimalCommand(animalGlobalId))
    );
}
