using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Defines the Map Game Participant contract.
/// </summary>
/// <summary>
/// Defines the Id0 contract.
/// </summary>
/// <summary>
/// Defines the Id1 contract.
/// </summary>
/// <summary>
/// Defines the Values contract.
/// </summary>
/// <summary>
/// Defines the Route Values contract.
/// </summary>
/// <summary>
/// Defines the Task Data Ids contract.
/// </summary>
/// <summary>
/// Defines the Name Present contract.
/// </summary>
/// <summary>
/// Defines the Name contract.
/// </summary>
/// <summary>
/// Defines the Optional Values contract.
/// </summary>
/// <summary>
/// Defines the Data Id contract.
/// </summary>
/// <summary>
/// Defines the Entries contract.
/// </summary>
public sealed record MapGameParticipant(
    [property: System.Text.Json.Serialization.JsonPropertyName("Id0")] LongId? Id0,
    [property: System.Text.Json.Serialization.JsonPropertyName("Id1")] LongId? Id1,
    int[] Values,
    int[] RouteValues,
    [property: System.Text.Json.Serialization.JsonPropertyName("TaskDataIds")] int[] TaskDataIds,
    bool NamePresent,
    string? Name,
    int[]? OptionalValues,
    [property: System.Text.Json.Serialization.JsonPropertyName("DataId")] int DataId,
    DataReferenceValue[] Entries
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameParticipant Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId? id0 = stream.ReadOptionalLongId();
        LongId? id1 = stream.ReadOptionalLongId();
        int[] values = stream.ReadVarIntArray(count: 5);
        int[] route = stream.ReadArray(static input => input.ReadVarInt());
        int[] tasks = stream.ReadArray(static input => input.ReadVarInt());
        bool namePresent = stream.ReadBoolean();
        string? name = namePresent ? stream.ReadOptionalString() : null;
        int[]? optional = stream.ReadBoolean() ? stream.ReadVarIntArray(count: 3) : null;

        return new MapGameParticipant(id0, id1, values, route, tasks, namePresent, name, optional, stream.ReadVarInt(), stream.ReadArray(DataReferenceValue.Decode));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongId(Id0);
        stream.WriteOptionalLongId(Id1);

        foreach (int value in Values)
            stream.WriteVarInt(value);

        stream.WriteArray(RouteValues, static (output, value) => output.WriteVarInt(value));
        stream.WriteArray(TaskDataIds, static (output, value) => output.WriteVarInt(value));
        stream.WriteBoolean(NamePresent);

        if (NamePresent)
            stream.WriteOptionalString(Name);

        stream.WriteBoolean(OptionalValues is not null);

        if (OptionalValues is { } values)
        {
            foreach (int value in values)
                stream.WriteVarInt(value);
        }

        stream.WriteVarInt(DataId);
        stream.WriteArray(Entries, static (output, value) => value.Encode(output));
    }
}
