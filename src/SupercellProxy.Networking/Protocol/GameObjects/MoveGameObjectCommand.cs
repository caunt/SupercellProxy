using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// <para>Moves an existing game object after validating its object-table category.</para>
/// </summary>
public sealed record MoveGameObjectCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 124;

    /// <summary>
    /// Initializes a new <see cref="MoveGameObjectCommand"/> instance.
    /// </summary>
    public MoveGameObjectCommand(int gameObjectGlobalId, int objectTableId, int logicX, int logicY)
    {
        GameObjectGlobalId = gameObjectGlobalId;
        ObjectTableId = objectTableId;
        PositionX = logicX;
        PositionY = logicY;
    }

    /// <summary>
    /// Gets the <c language="csharp">GameObjectGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">ObjectTableId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ObjectTableId")]
    public int ObjectTableId { get; }

    /// <summary>
    /// Gets the <c language="csharp">PositionX</c> value.
    /// </summary>
    public int PositionX { get; }

    /// <summary>
    /// Gets the <c language="csharp">PositionY</c> value.
    /// </summary>
    public int PositionY { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MoveGameObjectCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int gameObjectGlobalId = stream.ReadVarInt();
        int objectTableId = stream.ReadVarInt();
        int logicX = stream.ReadVarInt();
        int logicY = stream.ReadVarInt();

        return new MoveGameObjectCommand(gameObjectGlobalId, objectTableId, logicX, logicY);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GameObjectGlobalId);
        stream.WriteVarInt(ObjectTableId);
        stream.WriteVarInt(PositionX);
        stream.WriteVarInt(PositionY);
    }
}
