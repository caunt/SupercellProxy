using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// <para>Moves an existing game object after validating its object-table category.</para>
/// </summary>
public sealed record MoveGameObjectCommand : CodecCommand<MoveGameObjectCommand>, ICommandCodecProvider<MoveGameObjectCommand>
{
    /// <summary>
    /// Initializes a new <see cref="MoveGameObjectCommand"/> instance.
    /// </summary>
    public MoveGameObjectCommand(int gameObjectGlobalId, int objectTableId, int logicX, int logicY)
    {
        GameObjectGlobalId = gameObjectGlobalId;
        ObjectTableId = objectTableId;
        PositionX = logicX;
        PositionY = logicY;
    }

    /// <inheritdoc />
    public static CommandCodec<MoveGameObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GameObjectGlobalId,
            Codecs.VarInt,
            static command => command.ObjectTableId,
            Codecs.VarInt,
            static command => command.PositionX,
            Codecs.VarInt,
            static command => command.PositionY,
            static (gameObjectGlobalId, objectTableId, logicX, logicY) => new MoveGameObjectCommand(gameObjectGlobalId, objectTableId, logicX, logicY)
        )
    );

    /// <summary>
    /// Gets the <c language="csharp">GameObjectGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameObjectGlobalId")]
    public int GameObjectGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">ObjectTableId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ObjectTableId")]
    public int ObjectTableId { get; }

    /// <summary>
    /// Gets the <c language="csharp">PositionX</c> value.
    /// </summary>
    public int PositionX { get; }

    /// <summary>
    /// Gets the <c language="csharp">PositionY</c> value.
    /// </summary>
    public int PositionY { get; }
}
