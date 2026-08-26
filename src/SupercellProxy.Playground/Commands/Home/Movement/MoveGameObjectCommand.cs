using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Moves an existing game object after validating its object-table category.</para>
/// </summary>
public sealed record MoveGameObjectCommand : Command
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 124;

    /// <summary>
    /// Initializes a new <see cref="MoveGameObjectCommand"/> instance.
    /// </summary>
    public MoveGameObjectCommand(
        int gameObjectGlobalId,
        int objectTableId,
        int logicX,
        int logicY,
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executeSubTick, debugData0, debugData1)
    {
        GameObjectGlobalId = gameObjectGlobalId;
        ObjectTableId = objectTableId;
        PositionX = logicX;
        PositionY = logicY;
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c>GameObjectGlobalId</c> value.
    /// </summary>
    public int GameObjectGlobalId { get; }

    /// <summary>
    /// Gets the <c>ObjectTableId</c> value.
    /// </summary>
    public int ObjectTableId { get; }

    /// <summary>
    /// Gets the <c>PositionX</c> value.
    /// </summary>
    public int PositionX { get; }

    /// <summary>
    /// Gets the <c>PositionY</c> value.
    /// </summary>
    public int PositionY { get; }

    internal static MoveGameObjectCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var gameObjectGlobalId = stream.ReadVarInt();
        var objectTableId = stream.ReadVarInt();
        var logicX = stream.ReadVarInt();
        var logicY = stream.ReadVarInt();
        var fields = DecodeCommand(stream, environment);

        return new MoveGameObjectCommand(
            gameObjectGlobalId,
            objectTableId,
            logicX,
            logicY,
            fields.ExecuteSubTick,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GameObjectGlobalId);
        stream.WriteVarInt(ObjectTableId);
        stream.WriteVarInt(PositionX);
        stream.WriteVarInt(PositionY);
        EncodeCommand(stream, environment);
    }
}
