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
    [property: System.Text.Json.Serialization.JsonPropertyName("Id0")] LongIdentifier? Identifier0,
    [property: System.Text.Json.Serialization.JsonPropertyName("Id1")] LongIdentifier? Identifier1,
    int[] Values,
    int[] RouteValues,
    [property: System.Text.Json.Serialization.JsonPropertyName("TaskDataIds")] int[] TaskDataIdentifiers,
    bool NamePresent,
    string? Name,
    int[]? OptionalValues,
    [property: System.Text.Json.Serialization.JsonPropertyName("DataId")] int DataIdentifier,
    DataReferenceValue[] Entries
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameParticipant Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier? identifier0 = stream.ReadOptionalLongIdentifier();
        LongIdentifier? identifier1 = stream.ReadOptionalLongIdentifier();
        int[] values = stream.ReadVariableIntArray(count: 5);
        int[] route = stream.ReadArray(static input => input.ReadVariableInt());
        int[] tasks = stream.ReadArray(static input => input.ReadVariableInt());
        bool namePresent = stream.ReadBoolean();
        string? name = namePresent ? stream.ReadOptionalString() : null;
        int[]? optional = stream.ReadBoolean() ? stream.ReadVariableIntArray(count: 3) : null;

        return new MapGameParticipant(
            identifier0,
            identifier1,
            values,
            route,
            tasks,
            namePresent,
            name,
            optional,
            stream.ReadVariableInt(),
            stream.ReadArray(DataReferenceValue.Decode)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongIdentifier(Identifier0);
        stream.WriteOptionalLongIdentifier(Identifier1);

        foreach (int value in Values)
            stream.WriteVariableInt(value);

        stream.WriteArray(RouteValues, static (output, value) => output.WriteVariableInt(value));
        stream.WriteArray(TaskDataIdentifiers, static (output, value) => output.WriteVariableInt(value));
        stream.WriteBoolean(NamePresent);

        if (NamePresent)
            stream.WriteOptionalString(Name);

        stream.WriteBoolean(OptionalValues is not null);

        if (OptionalValues is { } values)
        {
            foreach (int value in values)
                stream.WriteVariableInt(value);
        }

        stream.WriteVariableInt(DataIdentifier);
        stream.WriteArray(Entries, static (output, value) => value.Encode(output));
    }
}
