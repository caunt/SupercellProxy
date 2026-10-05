using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


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
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.RecordStorageSignpostRankCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RecordStorageSignpostRankCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RecordStorageSignpostRankCommand(stream.ReadVarInt(), stream.ReadBoolean(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ObjectTableId);
        stream.WriteBoolean(MaterialsSignpost);
        stream.WriteVarInt(StorageGlobalId);
    }
}
