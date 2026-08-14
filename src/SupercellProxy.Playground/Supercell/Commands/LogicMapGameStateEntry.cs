using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native data-reference entry inside a map-game state.
/// </summary>
public sealed record LogicMapGameStateEntry
{
    public LogicMapGameStateEntry(
        int unknownGlobalId,
        int unknown0,
        LogicLong? unknownLogicLong,
        int unknown1,
        int unknown2,
        ReadOnlyMemory<LogicLong> unknownLogicLongs)
    {
        UnknownGlobalId = unknownGlobalId;
        Unknown0 = unknown0;
        UnknownLogicLong = unknownLogicLong;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        UnknownLogicLongs = unknownLogicLongs.ToArray();
    }

    public int UnknownGlobalId { get; }
    public int Unknown0 { get; }
    public LogicLong? UnknownLogicLong { get; }
    public int Unknown1 { get; }
    public int Unknown2 { get; }
    public ReadOnlyMemory<LogicLong> UnknownLogicLongs { get; }

    internal static LogicMapGameStateEntry Decode(SupercellStream stream)
    {
        return new LogicMapGameStateEntry(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            LogicMapGameWire.ReadOptionalLogicLong(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            LogicMapGameWire.ReadLogicLongs(stream));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(UnknownGlobalId);
        stream.WriteVarInt(Unknown0);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        LogicMapGameWire.WriteLogicLongs(stream, UnknownLogicLongs.Span);
    }
}
