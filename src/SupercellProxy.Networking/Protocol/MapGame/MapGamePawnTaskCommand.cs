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
    public MapGamePawnTaskCommand(MapGamePawn? pawn, MapGameTaskCollection? taskCollection)
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

        MapGamePawn? pawn = stream.ReadBoolean() ? MapGamePawn.Decode(stream) : null;

        MapGameTaskCollection? taskCollection = stream.ReadBoolean()
            ? MapGameTaskCollection.Decode(stream, dataResolver)
            : null;

        return new MapGamePawnTaskCommand(pawn, taskCollection);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(Pawn is not null);
        Pawn?.Encode(stream);
        stream.WriteBoolean(TaskCollection is not null);
        TaskCollection?.Encode(stream);
    }
}
