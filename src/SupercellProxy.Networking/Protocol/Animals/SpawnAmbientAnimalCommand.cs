using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Serverbound command that adds one decorative ambient animal (birds, butterflies, fish and the like) to the farm at a given spot.
/// The server refuses it once the farm already has the configured maximum of ambient animals.
/// </summary>
public sealed record SpawnAmbientAnimalCommand : CodecCommand<SpawnAmbientAnimalCommand>, ICommandCodecProvider<SpawnAmbientAnimalCommand>
{
    /// <summary>
    /// Describes one ambient animal to spawn.
    /// </summary>
    /// <param name="behavior">Movement pattern, 0 to 4; see <see cref="Behavior"/>.</param>
    /// <param name="horizontalCoordinate">Horizontal start position in farm coordinates.</param>
    /// <param name="verticalCoordinate">Vertical start position in farm coordinates.</param>
    /// <param name="destinationX">Horizontal position the animal heads for, for patterns that travel to a point.</param>
    /// <param name="destinationY">Vertical position the animal heads for, for patterns that travel to a point.</param>
    /// <param name="notifyListener">Whether native also tells its display listener about the new animal; no effect on game state.</param>
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
    /// Movement pattern the animal follows, 0 to 4; it selects which avoidance, attraction and landing points apply. Pattern 4 is
    /// the fish that swims near the boat dock. Anything outside 0 to 4 is invalid.
    /// </summary>
    public int Behavior { get; }

    /// <summary>
    /// Horizontal position the animal heads for, used by patterns that travel to a point.
    /// </summary>
    public int DestinationX { get; }

    /// <summary>
    /// Vertical position the animal heads for, used by patterns that travel to a point.
    /// </summary>
    public int DestinationY { get; }

    /// <summary>
    /// Horizontal start position in farm coordinates.
    /// </summary>
    [JsonPropertyName("X")]
    public int HorizontalCoordinate { get; }

    /// <summary>
    /// Whether native also tells its display listener about the new animal; it has no effect on game state.
    /// </summary>
    public bool NotifyListener { get; }

    /// <summary>
    /// Vertical start position in farm coordinates.
    /// </summary>
    [JsonPropertyName("Y")]
    public int VerticalCoordinate { get; }
}
