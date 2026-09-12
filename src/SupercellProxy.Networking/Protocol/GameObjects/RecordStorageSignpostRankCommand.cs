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
    [property: System.Text.Json.Serialization.JsonPropertyName("ObjectTableId")] int ObjectTableIdentifier,
    bool MaterialsSignpost,
    [property: System.Text.Json.Serialization.JsonPropertyName("StorageGlobalId")] int StorageGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
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
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new RecordStorageSignpostRankCommand(
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(ObjectTableIdentifier);
        stream.WriteBoolean(MaterialsSignpost);
        stream.WriteVariableInt(StorageGlobalIdentifier);
    }
}
