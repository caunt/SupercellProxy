using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.StructureFields;

/// <summary>
/// Represents <c language="csharp">CommandOptionalInt32StringField</c>.
/// </summary>
public sealed record CommandOptionalInt32StringField(int Value, string Text) : CommandField
{

    /// <summary>
    /// Gets the <c language="csharp">Empty</c> value.
    /// </summary>
    public static CommandOptionalInt32StringField Empty =>
        new(Value: 0, string.Empty) { HasValue = false };

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.OptionalInt32String;
    /// <summary>
    /// Gets or sets the <c language="csharp">HasValue</c> value.
    /// </summary>
    public bool HasValue { get; init; } = true;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandOptionalInt32StringField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.ReadBoolean()
            ? new CommandOptionalInt32StringField(stream.ReadInt32(), stream.ReadString())
            : Empty;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(HasValue);

        if (!HasValue)
            return;

        stream.WriteInt32(Value);
        stream.WriteString(Text);
    }
}
