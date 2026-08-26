using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandOptionalInt32StringField</c>.
/// </summary>
public sealed record CommandOptionalInt32StringField(int Value, string Text) : CommandField
{
    /// <summary>
    /// Gets or sets the <c>HasValue</c> value.
    /// </summary>
    public bool HasValue { get; init; } = true;
    internal override CommandFieldType FieldType => CommandFieldType.OptionalInt32String;

    /// <summary>
    /// Gets the <c>Empty</c> value.
    /// </summary>
    public static CommandOptionalInt32StringField Empty =>
        new(0, string.Empty) { HasValue = false };

    internal static CommandOptionalInt32StringField Decode(MessageStream stream) =>
        stream.ReadBoolean()
            ? new CommandOptionalInt32StringField(stream.ReadInt32(), stream.ReadString())
            : Empty;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(HasValue);

        if (!HasValue)
            return;

        stream.WriteInt32(Value);
        stream.WriteString(Text);
    }
}
