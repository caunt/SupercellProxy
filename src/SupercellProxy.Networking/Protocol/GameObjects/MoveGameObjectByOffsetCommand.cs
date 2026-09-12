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
        int gameObjectGlobalIdentifier,
        int logicOffsetX,
        int logicOffsetY,
        int expectedTileX,
        int expectedTileY,
        int expectedDataGlobalIdentifier,
        bool mirrored,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        GameObjectGlobalIdentifier = gameObjectGlobalIdentifier;
        OffsetX = logicOffsetX;
        OffsetY = logicOffsetY;
        ExpectedTileX = expectedTileX;
        ExpectedTileY = expectedTileY;
        ExpectedDataGlobalIdentifier = expectedDataGlobalIdentifier;
        Mirrored = mirrored;
    }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedDataGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ExpectedDataGlobalId")]
    public int ExpectedDataGlobalIdentifier { get; }

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
    public int GameObjectGlobalIdentifier { get; }

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
        int gameObjectGlobalIdentifier = stream.ReadVariableInt();
        int logicOffsetX = stream.ReadVariableInt();
        int logicOffsetY = stream.ReadVariableInt();
        int expectedTileX = stream.ReadVariableInt();
        int expectedTileY = stream.ReadVariableInt();
        int expectedDataGlobalIdentifier = stream.ReadVariableInt();
        bool mirrored = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new MoveGameObjectByOffsetCommand(
            gameObjectGlobalIdentifier,
            logicOffsetX,
            logicOffsetY,
            expectedTileX,
            expectedTileY,
            expectedDataGlobalIdentifier,
            mirrored,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(GameObjectGlobalIdentifier);
        stream.WriteVariableInt(OffsetX);
        stream.WriteVariableInt(OffsetY);
        stream.WriteVariableInt(ExpectedTileX);
        stream.WriteVariableInt(ExpectedTileY);
        stream.WriteVariableInt(ExpectedDataGlobalIdentifier);
        stream.WriteBoolean(Mirrored);
        EncodeCommand(stream, environment);
    }
}
