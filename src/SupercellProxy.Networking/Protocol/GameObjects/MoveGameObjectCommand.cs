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
    /// <summary>Initializes a new instance of the <see cref="MoveGameObjectCommand"/> record. Creates the command for one object.</summary>
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
    public static CommandCodec<MoveGameObjectCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.GameObjectGlobalId,
        Codecs.VarInt,
        static command => command.ObjectTableId,
        Codecs.VarInt,
        static command => command.PositionX,
        Codecs.VarInt,
        static command => command.PositionY,
        static (gameObjectGlobalId, objectTableId, logicX, logicY) => new MoveGameObjectCommand(gameObjectGlobalId, objectTableId, logicX, logicY)));

    /// <summary>Gets the global id of the object to move.</summary>
    /// <value>The id of a placed object in the home.</value>
    [JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>Gets the number of the data table that defines the moved object, sent so the server can tell which kind of object the id names.</summary>
    /// <value>The table part of the object's data id, for example 147 for gatherer nests such as beehives.</value>
    [JsonPropertyName("ObjectTableId")]
    public int ObjectTableId { get; }

    /// <summary>Gets the new horizontal position in map units (512 per tile).</summary>
    /// <value>The absolute horizontal coordinate the object is placed at, replacing its current one.</value>
    public int PositionX { get; }

    /// <summary>Gets the new vertical position in map units (512 per tile).</summary>
    /// <value>The absolute vertical coordinate the object is placed at, replacing its current one.</value>
    public int PositionY { get; }
}
