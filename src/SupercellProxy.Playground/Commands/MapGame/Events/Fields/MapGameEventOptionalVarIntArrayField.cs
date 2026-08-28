using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalVarIntArrayField</c>.
/// </summary>
internal sealed record MapGameEventOptionalVarIntArrayField : MapGameEventField
{
    /// <summary>
    /// Initializes a new <see cref="MapGameEventOptionalVarIntArrayField"/> instance.
    /// </summary>
    public MapGameEventOptionalVarIntArrayField(ReadOnlyMemory<int>? values) =>
        Values = values?.ToArray();

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int>? Values { get; }
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalVarIntArray;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Values is not null);

        if (Values is not null)
            new CommandVarIntArrayField(Values.Value).Encode(stream);
    }
}
