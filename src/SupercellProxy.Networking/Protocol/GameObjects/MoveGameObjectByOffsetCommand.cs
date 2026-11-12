using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Serverbound command shifting a placed object by an offset in map units (512 per tile) and setting its mirroring; the object
/// must still be the expected one on the expected tile.
/// </summary>
public sealed record MoveGameObjectByOffsetCommand : CodecCommand<MoveGameObjectByOffsetCommand>, ICommandCodecProvider<MoveGameObjectByOffsetCommand>
{
    /// <summary>Initializes a new instance of the <see cref="MoveGameObjectByOffsetCommand"/> record. Creates the command for one object.</summary>
    /// <param name="gameObjectGlobalId">Global id of the object to move.</param>
    /// <param name="logicOffsetX">Horizontal shift in map units.</param>
    /// <param name="logicOffsetY">Vertical shift in map units.</param>
    /// <param name="expectedTileX">Horizontal tile the object must stand on before the move.</param>
    /// <param name="expectedTileY">Vertical tile the object must stand on before the move.</param>
    /// <param name="expectedDataGlobalId">Global id of the table entry the object must have.</param>
    /// <param name="mirrored">Whether the object is mirrored after the move.</param>
    public MoveGameObjectByOffsetCommand(int gameObjectGlobalId,
        int logicOffsetX,
        int logicOffsetY,
        int expectedTileX,
        int expectedTileY,
        int expectedDataGlobalId,
        bool mirrored)
    {
        GameObjectGlobalId = gameObjectGlobalId;
        OffsetX = logicOffsetX;
        OffsetY = logicOffsetY;
        ExpectedTileX = expectedTileX;
        ExpectedTileY = expectedTileY;
        ExpectedDataGlobalId = expectedDataGlobalId;
        Mirrored = mirrored;
    }

    /// <inheritdoc />
    public static CommandCodec<MoveGameObjectByOffsetCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.GameObjectGlobalId,
        Codecs.VarInt,
        static command => command.OffsetX,
        Codecs.VarInt,
        static command => command.OffsetY,
        Codecs.VarInt,
        static command => command.ExpectedTileX,
        Codecs.VarInt,
        static command => command.ExpectedTileY,
        Codecs.VarInt,
        static command => command.ExpectedDataGlobalId,
        Codecs.Boolean,
        static command => command.Mirrored,
        static (gameObjectGlobalId, logicOffsetX, logicOffsetY, expectedTileX, expectedTileY, expectedDataGlobalId, mirrored) => new MoveGameObjectByOffsetCommand(gameObjectGlobalId, logicOffsetX, logicOffsetY, expectedTileX, expectedTileY, expectedDataGlobalId, mirrored)));

    /// <summary>Gets the global id of the object's table entry.</summary>
    /// <value>The data global id of the object's current table row.</value>
    [JsonPropertyName("ExpectedDataGlobalId")]
    public int ExpectedDataGlobalId { get; }

    /// <summary>Gets the horizontal tile the object stands on before the move.</summary>
    /// <value>The object's current horizontal position divided by 512 (whole tiles).</value>
    public int ExpectedTileX { get; }

    /// <summary>Gets the vertical tile the object stands on before the move.</summary>
    /// <value>The object's current vertical position divided by 512 (whole tiles).</value>
    public int ExpectedTileY { get; }

    /// <summary>Gets the global id of the object to move.</summary>
    /// <value>The id of a placed object in the home.</value>
    [JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>Gets whether the object is mirrored after the move.</summary>
    /// <value><see langword="true"/> if the object ends up mirrored; otherwise, <see langword="false"/>.</value>
    public bool Mirrored { get; }

    /// <summary>Gets the horizontal shift in map units (512 per tile).</summary>
    /// <value>A signed distance added to the object's current horizontal position; negative values move it toward lower X.</value>
    public int OffsetX { get; }

    /// <summary>Gets the vertical shift in map units (512 per tile).</summary>
    /// <value>A signed distance added to the object's current vertical position; negative values move it toward lower Y.</value>
    public int OffsetY { get; }
}
