using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Moves an existing game object by a logic-coordinate offset after validating its prior tile and data.</para>
/// </summary>
internal sealed record MoveGameObjectByOffsetCommand : Command
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
        bool mirrored,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
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
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">GameObjectGlobalId</c> value.
    /// </summary>
    public int GameObjectGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">OffsetX</c> value.
    /// </summary>
    public int OffsetX { get; }

    /// <summary>
    /// Gets the <c language="csharp">OffsetY</c> value.
    /// </summary>
    public int OffsetY { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedTileX</c> value.
    /// </summary>
    public int ExpectedTileX { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedTileY</c> value.
    /// </summary>
    public int ExpectedTileY { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedDataGlobalId</c> value.
    /// </summary>
    public int ExpectedDataGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Mirrored</c> value.
    /// </summary>
    public bool Mirrored { get; }

    internal static MoveGameObjectByOffsetCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var gameObjectGlobalId = stream.ReadVarInt();
        var logicOffsetX = stream.ReadVarInt();
        var logicOffsetY = stream.ReadVarInt();
        var expectedTileX = stream.ReadVarInt();
        var expectedTileY = stream.ReadVarInt();
        var expectedDataGlobalId = stream.ReadVarInt();
        var mirrored = stream.ReadBoolean();
        var fields = DecodeCommand(stream, environment);

        return new MoveGameObjectByOffsetCommand(
            gameObjectGlobalId,
            logicOffsetX,
            logicOffsetY,
            expectedTileX,
            expectedTileY,
            expectedDataGlobalId,
            mirrored,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GameObjectGlobalId);
        stream.WriteVarInt(OffsetX);
        stream.WriteVarInt(OffsetY);
        stream.WriteVarInt(ExpectedTileX);
        stream.WriteVarInt(ExpectedTileY);
        stream.WriteVarInt(ExpectedDataGlobalId);
        stream.WriteBoolean(Mirrored);
        EncodeCommand(stream, environment);
    }
}
