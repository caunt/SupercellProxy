using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Applies a server-approved name to a pet or sanctuary animal.</summary>
/// <param name="AnimalGlobalId">
/// Object id of the renamed animal, written as a 32-bit integer: a pet on the farm, or a sanctuary animal when the player is in their
/// town.
/// </param>
/// <param name="Name">The name the server accepted, written as a string after the id.</param>
/// <param name="Approved"><see langword="true"/> when the server accepted the name; <see langword="false"/> when it was rejected.</param>
public sealed record RenameAnimalServerCommand(int AnimalGlobalId, string Name, bool Approved) : CodecServerCommand<RenameAnimalServerCommand>, ICommandCodecProvider<RenameAnimalServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RenameAnimalServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Int32,
        static command => command.AnimalGlobalId,
        Codecs.String,
        static command => command.Name,
        Codecs.Boolean,
        static command => command.Approved,
        static (animalGlobalId, name, approved) => new RenameAnimalServerCommand(animalGlobalId, name, approved)));
}
