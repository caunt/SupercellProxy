using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameDumpTaskStatePayload : LogicMapGameTaskStatePayload
{
    public LogicMapGameDumpTaskStatePayload(
        ReadOnlyMemory<LogicCommandDataReferenceVarIntPair> values,
        ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? optionalValues,
        bool unknown0,
        LogicLong? unknownLogicLong,
        int unknownGlobalId0,
        int unknownGlobalId1)
    {
        Values = values.ToArray();
        OptionalValues = optionalValues?.ToArray();
        Unknown0 = unknown0;
        UnknownLogicLong = unknownLogicLong;
        UnknownGlobalId0 = unknownGlobalId0;
        UnknownGlobalId1 = unknownGlobalId1;
    }

    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair> Values { get; }
    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? OptionalValues { get; }
    public bool Unknown0 { get; }
    public LogicLong? UnknownLogicLong { get; }
    public int UnknownGlobalId0 { get; }
    public int UnknownGlobalId1 { get; }

    internal static LogicMapGameDumpTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameDumpTaskStatePayload(
            LogicMapGameWire.ReadDataReferenceVarIntPairs(stream),
            LogicMapGameWire.ReadOptionalDataReferenceVarIntPairs(stream),
            stream.ReadBoolean(),
            LogicMapGameWire.ReadOptionalLogicLong(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt());
    }

    internal override void Encode(SupercellStream stream)
    {
        LogicMapGameWire.WriteDataReferenceVarIntPairs(stream, Values.Span);
        LogicMapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
        stream.WriteBoolean(Unknown0);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong);
        stream.WriteVarInt(UnknownGlobalId0);
        stream.WriteVarInt(UnknownGlobalId1);
    }
}
