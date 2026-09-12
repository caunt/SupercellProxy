using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalVarIntArrayField</c>.
/// </summary>
public sealed record MapGameEventOptionalVariableIntArrayField : MapGameEventField
{
    /// <summary>
    /// Initializes a new <see cref="MapGameEventOptionalVariableIntArrayField"/> instance.
    /// </summary>
    public MapGameEventOptionalVariableIntArrayField(ReadOnlyMemory<int>? values)
    {
        Values = values is null ? null : (ReadOnlyMemory<int>?)values.Value.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalVariableIntArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int>? Values { get; }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Values is not null);

        if (Values is not null)
            new CommandVariableIntArrayField(Values.Value).Encode(stream);
    }
}
