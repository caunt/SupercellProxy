using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Logic command 321. The native class and semantic field names are not present in the stripped client.</para>
/// </summary>
public sealed record Command321 : Command
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 321;

    /// <summary>
    /// Initializes a new <see cref="Command321"/> instance.
    /// </summary>
    public Command321(
        MapGamePawn? pawn,
        MapGameTaskCollection? taskCollection,
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executeSubTick, debugData0, debugData1)
    {
        Pawn = pawn;
        TaskCollection = taskCollection;
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c>Pawn</c> value.
    /// </summary>
    public MapGamePawn? Pawn { get; }

    /// <summary>
    /// Gets the <c>TaskCollection</c> value.
    /// </summary>
    public MapGameTaskCollection? TaskCollection { get; }

    internal static Command321 Decode(
        MessageStream stream,
        CommandEnvironment environment,
        ICommandDataResolver? dataResolver
    )
    {
        var commandFields = DecodeCommand(stream, environment);
        var pawn = stream.ReadBoolean() ? MapGamePawn.Decode(stream) : null;
        var taskCollection = stream.ReadBoolean()
            ? MapGameTaskCollection.Decode(stream, dataResolver)
            : null;

        return new Command321(
            pawn,
            taskCollection,
            commandFields.ExecuteSubTick,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(Pawn is not null);
        Pawn?.Encode(stream);
        stream.WriteBoolean(TaskCollection is not null);
        TaskCollection?.Encode(stream);
    }
}
