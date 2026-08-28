using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native map-game configuration structure. Semantic names for its stripped scalar fields are not yet proven.</para>
/// </summary>
internal sealed record MapGameConfiguration
{
    /// <summary>
    /// Initializes a new <see cref="MapGameConfiguration"/> instance.
    /// </summary>
    public MapGameConfiguration(
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        int unknown5,
        ReadOnlyMemory<MapGameConfigurationEntry> entries,
        int unknown6,
        int unknown7,
        int unknown8
    )
    {
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        Unknown3 = unknown3;
        Unknown4 = unknown4;
        Unknown5 = unknown5;
        Entries = entries.ToArray();
        Unknown6 = unknown6;
        Unknown7 = unknown7;
        Unknown8 = unknown8;
    }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown5</c> value.
    /// </summary>
    public int Unknown5 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Entries</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameConfigurationEntry> Entries { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown6</c> value.
    /// </summary>
    public int Unknown6 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown7</c> value.
    /// </summary>
    public int Unknown7 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown8</c> value.
    /// </summary>
    public int Unknown8 { get; }

    internal static MapGameConfiguration Decode(MessageStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var unknown4 = stream.ReadVarInt();
        var unknown5 = stream.ReadVarInt();
        var entryCount = MapGameWire.ReadCount(stream, "configuration entry");
        var entries = new MapGameConfigurationEntry[entryCount];

        for (var i = 0; i < entries.Length; i++)
            entries[i] = MapGameConfigurationEntry.Decode(stream);

        return new MapGameConfiguration(
            unknown0,
            unknown1,
            unknown2,
            unknown3,
            unknown4,
            unknown5,
            entries,
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteVarInt(Entries.Length);

        foreach (var entry in Entries.Span)
            entry.Encode(stream);

        stream.WriteVarInt(Unknown6);
        stream.WriteVarInt(Unknown7);
        stream.WriteVarInt(Unknown8);
    }
}
