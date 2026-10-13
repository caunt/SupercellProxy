using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Asks the server to name a pet or sanctuary animal; the approved name returns as <see cref="RenameAnimalServerCommand"/>.</summary>
/// <param name="Name">Gets the requested animal name.</param>
/// <param name="AnimalGlobalId">Gets the animal game object id.</param>
public sealed record EditPetNameMessage(string Name, int AnimalGlobalId) : CodecMessage<EditPetNameMessage>, ICodecProvider<EditPetNameMessage>
{
    /// <inheritdoc />
    public static ICodec<EditPetNameMessage> Codec { get; } = Codecs.Composite(
        Codecs.String,
        static message => message.Name,
        Codecs.Int32,
        static message => message.AnimalGlobalId,
        static (name, animal) => new EditPetNameMessage(name, animal)
    );
}
