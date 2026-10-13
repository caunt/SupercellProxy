using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Puts an accessory theme on an animal.</summary>
/// <param name="AnimalGlobalId">The animal game object id.</param>
/// <param name="AccessoryGlobalId">The accessory data id.</param>
/// <param name="ThemeGlobalId">The accessory theme data id.</param>
public sealed record SetAnimalAccessoryCommand(int AnimalGlobalId, int AccessoryGlobalId, int ThemeGlobalId) : CodecCommand<SetAnimalAccessoryCommand>, ICommandCodecProvider<SetAnimalAccessoryCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetAnimalAccessoryCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            Codecs.VarInt,
            static command => command.AccessoryGlobalId,
            Codecs.VarInt,
            static command => command.ThemeGlobalId,
            static (animalGlobalId, accessoryGlobalId, themeGlobalId) => new SetAnimalAccessoryCommand(animalGlobalId, accessoryGlobalId, themeGlobalId)
        )
    );
}
