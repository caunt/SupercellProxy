using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Circus;

/// <summary>Gives a circus animal one of the items it asks for and grants the animal's reward.</summary>
/// <param name="CircusAnimalGlobalId">Gets the circus animal game object id.</param>
/// <param name="ItemGlobalId">Gets the given item data id.</param>
public sealed record FeedCircusAnimalCommand(int CircusAnimalGlobalId, int ItemGlobalId) : CodecCommand<FeedCircusAnimalCommand>, ICommandCodecProvider<FeedCircusAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FeedCircusAnimalCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CircusAnimalGlobalId,
            Codecs.VarInt,
            static command => command.ItemGlobalId,
            static (circusAnimalGlobalId, itemGlobalId) => new FeedCircusAnimalCommand(circusAnimalGlobalId, itemGlobalId)
        )
    );
}
