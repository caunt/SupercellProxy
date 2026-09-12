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
    public MoveGameObjectCommand(
        int gameObjectGlobalIdentifier,
        int objectTableIdentifier,
        int logicX,
        int logicY,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        GameObjectGlobalIdentifier = gameObjectGlobalIdentifier;
        ObjectTableIdentifier = objectTableIdentifier;
        PositionX = logicX;
        PositionY = logicY;
    }

    /// <summary>
    /// Gets the <c language="csharp">GameObjectGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">ObjectTableId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ObjectTableId")]
    public int ObjectTableIdentifier { get; }

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
        int gameObjectGlobalIdentifier = stream.ReadVariableInt();
        int objectTableIdentifier = stream.ReadVariableInt();
        int logicX = stream.ReadVariableInt();
        int logicY = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new MoveGameObjectCommand(
            gameObjectGlobalIdentifier,
            objectTableIdentifier,
            logicX,
            logicY,
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
        stream.WriteVariableInt(ObjectTableIdentifier);
        stream.WriteVariableInt(PositionX);
        stream.WriteVariableInt(PositionY);
        EncodeCommand(stream, environment);
    }
}
