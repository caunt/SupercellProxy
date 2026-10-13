using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects.Placement;

/// <summary>Flips a placed game object horizontally when the mirrored footprint still fits.</summary>
/// <param name="ObjectGlobalId">The game object id.</param>
public sealed record MirrorGameObjectCommand(int ObjectGlobalId) : CodecCommand<MirrorGameObjectCommand>, ICommandCodecProvider<MirrorGameObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MirrorGameObjectCommand> Codec { get; } = Codecs.Command(
        type: 53,
        Codecs.Composite(Codecs.VarInt, static command => command.ObjectGlobalId, static (objectGlobalId) => new MirrorGameObjectCommand(objectGlobalId))
    );
}
