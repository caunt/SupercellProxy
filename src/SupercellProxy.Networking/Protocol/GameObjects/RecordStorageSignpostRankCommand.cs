using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Record Storage Signpost Rank Command contract.
/// </summary>
/// <summary>
/// Defines the Object Table Id contract.
/// </summary>
/// <summary>
/// Defines the Materials Signpost contract.
/// </summary>
/// <summary>
/// Defines the Storage Global Id contract.
/// </summary>
public sealed record RecordStorageSignpostRankCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("ObjectTableId")] int ObjectTableId,
    bool MaterialsSignpost,
    [property: System.Text.Json.Serialization.JsonPropertyName("StorageGlobalId")] int StorageGlobalId
)
    : CodecCommand<RecordStorageSignpostRankCommand>, ICommandCodecProvider<RecordStorageSignpostRankCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordStorageSignpostRankCommand> Codec { get; } = Codecs.Command(
        type: 594,
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
