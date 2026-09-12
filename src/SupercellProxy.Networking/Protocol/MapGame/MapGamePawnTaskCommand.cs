using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Logic command 321. The native class and semantic field names are not present in the stripped client.</para>
/// </summary>
public sealed record MapGamePawnTaskCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 321;

    /// <summary>
    /// Initializes a new <see cref="MapGamePawnTaskCommand"/> instance.
    /// </summary>
    public MapGamePawnTaskCommand(
        MapGamePawn? pawn,
        MapGameTaskCollection? taskCollection,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        Pawn = pawn;
        TaskCollection = taskCollection;
    }

    /// <summary>
    /// Gets the <c language="csharp">Pawn</c> value.
    /// </summary>
    public MapGamePawn? Pawn { get; }

    /// <summary>
    /// Gets the <c language="csharp">TaskCollection</c> value.
    /// </summary>
    public MapGameTaskCollection? TaskCollection { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGamePawnTaskCommand Decode(MessageStream stream, CommandEnvironment environment, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);
        MapGamePawn? pawn = stream.ReadBoolean() ? MapGamePawn.Decode(stream) : null;

        MapGameTaskCollection? taskCollection = stream.ReadBoolean()
            ? MapGameTaskCollection.Decode(stream, dataResolver)
            : null;

        return new MapGamePawnTaskCommand(pawn, taskCollection, commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(Pawn is not null);
        Pawn?.Encode(stream);
        stream.WriteBoolean(TaskCollection is not null);
        TaskCollection?.Encode(stream);
    }
}
