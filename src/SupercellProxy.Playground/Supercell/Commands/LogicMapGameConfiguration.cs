using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native map-game configuration structure. Semantic names for its stripped scalar fields are not yet proven.
/// </summary>
public sealed record LogicMapGameConfiguration
{
    public LogicMapGameConfiguration(
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        int unknown5,
        ReadOnlyMemory<LogicMapGameConfigurationEntry> entries,
        int unknown6,
        int unknown7,
        int unknown8)
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

    public int Unknown0 { get; }
    public int Unknown1 { get; }
    public int Unknown2 { get; }
    public int Unknown3 { get; }
    public int Unknown4 { get; }
    public int Unknown5 { get; }
    public ReadOnlyMemory<LogicMapGameConfigurationEntry> Entries { get; }
    public int Unknown6 { get; }
    public int Unknown7 { get; }
    public int Unknown8 { get; }

    internal static LogicMapGameConfiguration Decode(SupercellStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var unknown4 = stream.ReadVarInt();
        var unknown5 = stream.ReadVarInt();
        var entryCount = LogicMapGameWire.ReadCount(stream, "configuration entry");
        var entries = new LogicMapGameConfigurationEntry[entryCount];

        for (var i = 0; i < entries.Length; i++)
            entries[i] = LogicMapGameConfigurationEntry.Decode(stream);

        return new LogicMapGameConfiguration(
            unknown0,
            unknown1,
            unknown2,
            unknown3,
            unknown4,
            unknown5,
            entries,
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
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
