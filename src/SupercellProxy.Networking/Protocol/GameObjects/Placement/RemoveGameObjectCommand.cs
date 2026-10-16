using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects.Placement;

/// <summary>Removes a removable game object from the home.</summary>
/// <param name="ObjectGlobalId">The game object id.</param>
public sealed record RemoveGameObjectCommand(int ObjectGlobalId) : CodecCommand<RemoveGameObjectCommand>, ICommandCodecProvider<RemoveGameObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoveGameObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ObjectGlobalId, static objectGlobalId => new RemoveGameObjectCommand(objectGlobalId))
    );
}
