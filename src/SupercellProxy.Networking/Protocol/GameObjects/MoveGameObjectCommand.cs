using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Serverbound command placing an existing object at a new map position (512 map units per tile); the object's table must
/// match.
/// </summary>
public sealed record MoveGameObjectCommand : CodecCommand<MoveGameObjectCommand>, ICommandCodecProvider<MoveGameObjectCommand>
{
    /// <summary>Creates the command for one object.</summary>
    /// <param name="gameObjectGlobalId">Global id of the object to move.</param>
    /// <param name="objectTableId">Table id of the object; it must match the object.</param>
    /// <param name="logicX">New horizontal position in map units.</param>
    /// <param name="logicY">New vertical position in map units.</param>
    public MoveGameObjectCommand(int gameObjectGlobalId, int objectTableId, int logicX, int logicY)
    {
        GameObjectGlobalId = gameObjectGlobalId;
        ObjectTableId = objectTableId;
        PositionX = logicX;
        PositionY = logicY;
    }

    /// <inheritdoc />
    public static CommandCodec<MoveGameObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GameObjectGlobalId,
            Codecs.VarInt,
            static command => command.ObjectTableId,
            Codecs.VarInt,
            static command => command.PositionX,
            Codecs.VarInt,
            static command => command.PositionY,
            static (gameObjectGlobalId, objectTableId, logicX, logicY) => new MoveGameObjectCommand(gameObjectGlobalId, objectTableId, logicX, logicY)
        )
    );

    /// <summary>Global id of the object to move.</summary>
    [JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>Table id of the object; it must match the object.</summary>
    [JsonPropertyName("ObjectTableId")]
    public int ObjectTableId { get; }

    /// <summary>New horizontal position in map units (512 per tile).</summary>
    public int PositionX { get; }

    /// <summary>New vertical position in map units (512 per tile).</summary>
    public int PositionY { get; }
}
