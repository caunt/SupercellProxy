using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Taps a sanctuary animal.</summary>
/// <param name="AnimalGlobalId">The sanctuary animal game object id.</param>
public sealed record TapSanctuaryAnimalCommand(int AnimalGlobalId) : CodecCommand<TapSanctuaryAnimalCommand>, ICommandCodecProvider<TapSanctuaryAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TapSanctuaryAnimalCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.AnimalGlobalId, static animalGlobalId => new TapSanctuaryAnimalCommand(animalGlobalId))
    );
}
