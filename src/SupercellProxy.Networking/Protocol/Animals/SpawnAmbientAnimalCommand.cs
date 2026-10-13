using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Defines the Spawn Ambient Animal Command contract.
/// </summary>
public sealed record SpawnAmbientAnimalCommand : CodecCommand<SpawnAmbientAnimalCommand>, ICommandCodecProvider<SpawnAmbientAnimalCommand>
{
    /// <summary>
    /// Provides the Spawn Ambient Animal Command value or operation.
    /// </summary>
    public SpawnAmbientAnimalCommand(int behavior, int horizontalCoordinate, int verticalCoordinate, int destinationX, int destinationY, bool notifyListener)
    {
        Behavior = behavior;
        HorizontalCoordinate = horizontalCoordinate;
        VerticalCoordinate = verticalCoordinate;
        DestinationX = destinationX;
        DestinationY = destinationY;
        NotifyListener = notifyListener;
    }

    /// <inheritdoc />
    public static CommandCodec<SpawnAmbientAnimalCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Behavior,
            Codecs.VarInt,
            static command => command.HorizontalCoordinate,
            Codecs.VarInt,
            static command => command.VerticalCoordinate,
            Codecs.VarInt,
            static command => command.DestinationX,
            Codecs.VarInt,
            static command => command.DestinationY,
            Codecs.Boolean,
            static command => command.NotifyListener,
            static (behavior, horizontalCoordinate, verticalCoordinate, destinationX, destinationY, notifyListener) => new SpawnAmbientAnimalCommand(behavior, horizontalCoordinate, verticalCoordinate, destinationX, destinationY, notifyListener)
        )
    );

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
    /// Gets the Y value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Y")]
    public int VerticalCoordinate { get; }
}
