using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects.Placement;

/// <summary>Moves a placed game object into storage.</summary>
/// <param name="ObjectGlobalId">The game object id.</param>
public sealed record StoreGameObjectCommand(int ObjectGlobalId) : CodecCommand<StoreGameObjectCommand>, ICommandCodecProvider<StoreGameObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StoreGameObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ObjectGlobalId, static (objectGlobalId) => new StoreGameObjectCommand(objectGlobalId))
    );
}
