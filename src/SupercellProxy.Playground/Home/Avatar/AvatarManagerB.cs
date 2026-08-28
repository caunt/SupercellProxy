using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">AvatarManagerB</c>.
/// </summary>
internal sealed record AvatarManagerB
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Version</c> value.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Entries</c> value.
    /// </summary>
    public AvatarManagerBEntry[] Entries { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public AvatarManagerBState State { get; init; } = new(0, 0, 0, 0);

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries0</c> value.
    /// </summary>
    public AvatarManagerBMapEntry[] UnknownEntries0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries1</c> value.
    /// </summary>
    public AvatarManagerBMapEntry[] UnknownEntries1 { get; init; } = [];

    internal static AvatarManagerB Decode(MessageStream stream)
    {
        var version = stream.ReadVarInt();

        if (version <= 0)
            return new AvatarManagerB { Version = version };

        return new AvatarManagerB
        {
            Version = version,
            Entries = stream.ReadArray(AvatarManagerBEntry.Decode),
            State = AvatarManagerBState.Decode(stream),
            UnknownEntries0 = stream.ReadArray(AvatarManagerBMapEntry.Decode),
            UnknownEntries1 = stream.ReadArray(AvatarManagerBMapEntry.Decode),
        };
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Version);

        if (Version <= 0)
            return;

        stream.WriteArray(Entries, static (valueStream, value) => value.Encode(valueStream));
        State.Encode(stream);
        stream.WriteArray(
            UnknownEntries0,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteArray(
            UnknownEntries1,
            static (valueStream, value) => value.Encode(valueStream)
        );
    }
}
