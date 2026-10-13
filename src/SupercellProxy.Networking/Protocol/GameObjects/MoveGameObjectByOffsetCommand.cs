using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// <para>Moves an existing game object by a logic-coordinate offset after validating its prior tile and data.</para>
/// </summary>
public sealed record MoveGameObjectByOffsetCommand : CodecCommand<MoveGameObjectByOffsetCommand>, ICommandCodecProvider<MoveGameObjectByOffsetCommand>
{
    /// <summary>
    /// Initializes a new <see cref="MoveGameObjectByOffsetCommand"/> instance.
    /// </summary>
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

    /// <summary>
    /// Gets the <c language="csharp">ExpectedDataGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ExpectedDataGlobalId")]
    public int ExpectedDataGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedTileX</c> value.
    /// </summary>
    public int ExpectedTileX { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpectedTileY</c> value.
    /// </summary>
    public int ExpectedTileY { get; }

    /// <summary>
    /// Gets the <c language="csharp">GameObjectGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Mirrored</c> value.
    /// </summary>
    public bool Mirrored { get; }

    /// <summary>
    /// Gets the <c language="csharp">OffsetX</c> value.
    /// </summary>
    public int OffsetX { get; }

    /// <summary>
    /// Gets the <c language="csharp">OffsetY</c> value.
    /// </summary>
    public int OffsetY { get; }
}
