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
    /// Initializes a new instance of the <see cref="SpawnAmbientAnimalCommand"/> record. Describes one ambient animal to spawn.
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
    public static CommandCodec<SpawnAmbientAnimalCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
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
        static (behavior, horizontalCoordinate, verticalCoordinate, destinationX, destinationY, notifyListener) => new SpawnAmbientAnimalCommand(behavior, horizontalCoordinate, verticalCoordinate, destinationX, destinationY, notifyListener)));

    /// <summary>
    /// Gets the movement pattern the animal follows, 0 to 4. Pattern 4 is
    /// the fish that swims near the boat dock. Anything outside 0 to 4 is invalid.
    /// </summary>
    /// <value>The first VarInt of the command, as given to the constructor; it is sent unchecked, so a value outside 0 to 4 reaches the server as is.</value>
    public int Behavior { get; }

    /// <summary>
    /// Gets the horizontal position the animal heads for, used by patterns that travel to a point.
    /// </summary>
    /// <value>The fourth VarInt of the command, in the same farm coordinates as <see cref="HorizontalCoordinate"/>.</value>
    public int DestinationX { get; }

    /// <summary>
    /// Gets the vertical position the animal heads for, used by patterns that travel to a point.
    /// </summary>
    /// <value>The fifth VarInt of the command, in the same farm coordinates as <see cref="VerticalCoordinate"/>.</value>
    public int DestinationY { get; }

    /// <summary>
    /// Gets the horizontal start position in farm coordinates.
    /// </summary>
    /// <value>The second VarInt of the command, where the animal appears; JSON names it <c>X</c>.</value>
    [JsonPropertyName("X")]
    public int HorizontalCoordinate { get; }

    /// <summary>
    /// Gets whether the new animal is also announced to the display.
    /// </summary>
    /// <value><see langword="true"/> if the command's closing boolean is set; otherwise, <see langword="false"/>.</value>
    public bool NotifyListener { get; }

    /// <summary>
    /// Gets the vertical start position in farm coordinates.
    /// </summary>
    /// <value>The third VarInt of the command, where the animal appears; JSON names it <c>Y</c>.</value>
    [JsonPropertyName("Y")]
    public int VerticalCoordinate { get; }
}
