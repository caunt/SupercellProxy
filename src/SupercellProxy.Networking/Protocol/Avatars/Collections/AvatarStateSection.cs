using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarStateSection</c>.
/// </summary>
public sealed record AvatarStateSection
{

    /// <summary>
    /// Gets or sets the <c language="csharp">Entries</c> value.
    /// </summary>
    public AvatarStateEntry[] Entries { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public AvatarStateValues State { get; init; } = new(Unknown0: 0, Unknown1: 0, Unknown2: 0, Unknown3: 0);

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries0</c> value.
    /// </summary>
    public AvatarStateMapEntry[] UnknownEntries0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries1</c> value.
    /// </summary>
    public AvatarStateMapEntry[] UnknownEntries1 { get; init; } = [];
    /// <summary>
    /// Gets or sets the <c language="csharp">Version</c> value.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarStateSection Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int version = stream.ReadVariableInt();

        return version <= 0
            ? new AvatarStateSection { Version = version }
            : new AvatarStateSection
            {
                Version = version,
                Entries = stream.ReadArray(AvatarStateEntry.Decode),
                State = AvatarStateValues.Decode(stream),
                UnknownEntries0 = stream.ReadArray(AvatarStateMapEntry.Decode),
                UnknownEntries1 = stream.ReadArray(AvatarStateMapEntry.Decode),
            };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Version);

        if (Version <= 0)
            return;

        stream.WriteArray(Entries, static (valueStream, value) => value.Encode(valueStream));
        State.Encode(stream);
        stream.WriteArray(UnknownEntries0, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(UnknownEntries1, static (valueStream, value) => value.Encode(valueStream));
    }
}
