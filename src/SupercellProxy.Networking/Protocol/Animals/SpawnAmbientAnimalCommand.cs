using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Defines the Spawn Ambient Animal Command contract.
/// </summary>
public sealed record SpawnAmbientAnimalCommand : Command
{
    /// <summary>
    /// Provides the Command Type value or operation.
    /// </summary>
    public const int CommandType = 50;

    /// <summary>
    /// Provides the Spawn Ambient Animal Command value or operation.
    /// </summary>
    public SpawnAmbientAnimalCommand(
        int behavior,
        int horizontalCoordinate,
        int verticalCoordinate,
        int destinationX,
        int destinationY,
        bool notifyListener,
        int executionPhaseCounter = 0,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        Behavior = behavior;
        HorizontalCoordinate = horizontalCoordinate;
        VerticalCoordinate = verticalCoordinate;
        DestinationX = destinationX;
        DestinationY = destinationY;
        NotifyListener = notifyListener;
    }

    /// <summary>
    /// Gets the Behavior value.
    /// </summary>
    public int Behavior { get; }

    /// <summary>
    /// Gets the Destination X value.
    /// </summary>
    public int DestinationX { get; }

    /// <summary>
    /// Gets the Destination Y value.
    /// </summary>
    public int DestinationY { get; }

    /// <summary>
    /// Gets the X value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("X")]
    public int HorizontalCoordinate { get; }

    /// <summary>
    /// Gets the Notify Listener value.
    /// </summary>
    public bool NotifyListener { get; }

    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the Y value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Y")]
    public int VerticalCoordinate { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static SpawnAmbientAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int behavior = stream.ReadVariableInt();
        int horizontalCoordinate = stream.ReadVariableInt();
        int verticalCoordinate = stream.ReadVariableInt();
        int destinationX = stream.ReadVariableInt();
        int destinationY = stream.ReadVariableInt();
        bool notifyListener = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new SpawnAmbientAnimalCommand(
            behavior,
            horizontalCoordinate,
            verticalCoordinate,
            destinationX,
            destinationY,
            notifyListener,
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
        stream.WriteVariableInt(Behavior);
        stream.WriteVariableInt(HorizontalCoordinate);
        stream.WriteVariableInt(VerticalCoordinate);
        stream.WriteVariableInt(DestinationX);
        stream.WriteVariableInt(DestinationY);
        stream.WriteBoolean(NotifyListener);
        EncodeCommand(stream, environment);
    }
}
