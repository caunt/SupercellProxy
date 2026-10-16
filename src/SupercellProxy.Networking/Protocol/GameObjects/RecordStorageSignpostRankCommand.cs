using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Serverbound command recording the barn or silo rank the player has seen on one of its signposts, so the signpost stops
/// announcing that rank.
/// </summary>
/// <param name="ObjectTableId">Table id of the storage building; it must match the building.</param>
/// <param name="MaterialsSignpost">Whether this is the upgrade-materials signpost; otherwise it is the upgrade signpost.</param>
/// <param name="StorageGlobalId">Global id of the barn's or silo's game object.</param>
public sealed record RecordStorageSignpostRankCommand(
    [property: JsonPropertyName("ObjectTableId")] int ObjectTableId,
    bool MaterialsSignpost,
    [property: JsonPropertyName("StorageGlobalId")] int StorageGlobalId
)
    : CodecCommand<RecordStorageSignpostRankCommand>, ICommandCodecProvider<RecordStorageSignpostRankCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordStorageSignpostRankCommand> Codec { get; } = Codecs.Command(
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.MaterialsSignpost,
                Codecs.VarInt,
                static command => command.StorageGlobalId,
                Codecs.VarInt,
                static command => command.ObjectTableId,
                static (materialsSignpost, storageGlobalId, objectTableId) => new RecordStorageSignpostRankCommand(objectTableId, materialsSignpost, storageGlobalId)
            ),
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.ObjectTableId,
                Codecs.Boolean,
                static command => command.MaterialsSignpost,
                Codecs.VarInt,
                static command => command.StorageGlobalId,
                static (objectTableId, materialsSignpost, storageGlobalId) => new RecordStorageSignpostRankCommand(objectTableId, materialsSignpost, storageGlobalId)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
