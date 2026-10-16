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
    /// <summary>Creates the command for one object.</summary>
    /// <param name="gameObjectGlobalId">Global id of the object to move.</param>
    /// <param name="logicOffsetX">Horizontal shift in map units.</param>
    /// <param name="logicOffsetY">Vertical shift in map units.</param>
    /// <param name="expectedTileX">Horizontal tile the object must stand on before the move.</param>
    /// <param name="expectedTileY">Vertical tile the object must stand on before the move.</param>
    /// <param name="expectedDataGlobalId">Global id of the table entry the object must have.</param>
    /// <param name="mirrored">Whether the object is mirrored after the move.</param>
    public MoveGameObjectByOffsetCommand(
        int gameObjectGlobalId,
        int logicOffsetX,
        int logicOffsetY,
        int expectedTileX,
        int expectedTileY,
        int expectedDataGlobalId,
        bool mirrored
    )
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
    public static CommandCodec<MoveGameObjectByOffsetCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
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
            static (gameObjectGlobalId, logicOffsetX, logicOffsetY, expectedTileX, expectedTileY, expectedDataGlobalId, mirrored) => new MoveGameObjectByOffsetCommand(gameObjectGlobalId, logicOffsetX, logicOffsetY, expectedTileX, expectedTileY, expectedDataGlobalId, mirrored)
        )
    );

    /// <summary>Global id of the table entry the object must have.</summary>
    [JsonPropertyName("ExpectedDataGlobalId")]
    public int ExpectedDataGlobalId { get; }

    /// <summary>Horizontal tile the object must stand on before the move.</summary>
    public int ExpectedTileX { get; }

    /// <summary>Vertical tile the object must stand on before the move.</summary>
    public int ExpectedTileY { get; }

    /// <summary>Global id of the object to move.</summary>
    [JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>Whether the object is mirrored after the move.</summary>
    public bool Mirrored { get; }

    /// <summary>Horizontal shift in map units (512 per tile).</summary>
    public int OffsetX { get; }

    /// <summary>Vertical shift in map units (512 per tile).</summary>
    public int OffsetY { get; }
}
