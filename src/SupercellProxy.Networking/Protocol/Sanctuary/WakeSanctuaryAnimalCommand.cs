using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Wakes a sleeping sanctuary animal.</summary>
/// <param name="AnimalGlobalId">The sanctuary animal game object id.</param>
public sealed record WakeSanctuaryAnimalCommand(int AnimalGlobalId) : CodecCommand<WakeSanctuaryAnimalCommand>, ICommandCodecProvider<WakeSanctuaryAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<WakeSanctuaryAnimalCommand> Codec { get; } = Codecs.Command(
        type: 219,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            static (animalGlobalId) => new WakeSanctuaryAnimalCommand(animalGlobalId)
        )
    );
}
