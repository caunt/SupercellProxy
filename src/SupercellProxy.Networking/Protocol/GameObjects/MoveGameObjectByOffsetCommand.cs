using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// <para>Moves an existing game object by a logic-coordinate offset after validating its prior tile and data.</para>
/// </summary>
public sealed record MoveGameObjectByOffsetCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 3;

    /// <summary>
    /// Initializes a new <see cref="MoveGameObjectByOffsetCommand"/> instance.
    /// </summary>
    public MoveGameObjectByOffsetCommand(
        int gameObjectGlobalId,
        int logicOffsetX,
        int logicOffsetY,
        int expectedTileX,
        int expectedTileY,
        int expectedDataGlobalId,
        bool mirrored
    )
    {
        GameObjectGlobalId = gameObjectGlobalId;
        OffsetX = logicOffsetX;
        OffsetY = logicOffsetY;
        ExpectedTileX = expectedTileX;
        ExpectedTileY = expectedTileY;
        ExpectedDataGlobalId = expectedDataGlobalId;
        Mirrored = mirrored;
    }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedDataGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ExpectedDataGlobalId")]
    public int ExpectedDataGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedTileX</c> value.
    /// </summary>
    public int ExpectedTileX { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedTileY</c> value.
    /// </summary>
    public int ExpectedTileY { get; }

    /// <summary>
    /// Gets the <c language="csharp">GameObjectGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Mirrored</c> value.
    /// </summary>
    public bool Mirrored { get; }

    /// <summary>
    /// Gets the <c language="csharp">OffsetX</c> value.
    /// </summary>
    public int OffsetX { get; }

    /// <summary>
    /// Gets the <c language="csharp">OffsetY</c> value.
    /// </summary>
    public int OffsetY { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MoveGameObjectByOffsetCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int gameObjectGlobalId = stream.ReadVarInt();
        int logicOffsetX = stream.ReadVarInt();
        int logicOffsetY = stream.ReadVarInt();
        int expectedTileX = stream.ReadVarInt();
        int expectedTileY = stream.ReadVarInt();
        int expectedDataGlobalId = stream.ReadVarInt();
        bool mirrored = stream.ReadBoolean();

        return new MoveGameObjectByOffsetCommand(gameObjectGlobalId, logicOffsetX, logicOffsetY, expectedTileX, expectedTileY, expectedDataGlobalId, mirrored);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GameObjectGlobalId);
        stream.WriteVarInt(OffsetX);
        stream.WriteVarInt(OffsetY);
        stream.WriteVarInt(ExpectedTileX);
        stream.WriteVarInt(ExpectedTileY);
        stream.WriteVarInt(ExpectedDataGlobalId);
        stream.WriteBoolean(Mirrored);
    }
}
