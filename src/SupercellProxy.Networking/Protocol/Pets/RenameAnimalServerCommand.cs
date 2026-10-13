using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Applies a server-approved name to a pet or sanctuary animal.</summary>
public sealed record RenameAnimalServerCommand(int AnimalGlobalId, string Name, bool Approved) : CodecServerCommand<RenameAnimalServerCommand>, ICommandCodecProvider<RenameAnimalServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RenameAnimalServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.AnimalGlobalId,
            Codecs.String,
            static command => command.Name,
            Codecs.Boolean,
            static command => command.Approved,
            static (animalGlobalId, name, approved) => new RenameAnimalServerCommand(animalGlobalId, name, approved)
        )
    );
}
