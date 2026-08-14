using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameChickenTaskStatePayload : LogicMapGameTaskStatePayload
{
    public LogicMapGameChickenTaskStatePayload(
        int unknown0,
        bool unknownBoolean0,
        LogicLong? unknownLogicLong,
        ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? optionalValues,
        ReadOnlyMemory<LogicLong> logicLongs)
    {
        Unknown0 = unknown0;
        UnknownBoolean0 = unknownBoolean0;
        UnknownLogicLong = unknownLogicLong;
        OptionalValues = optionalValues?.ToArray();
        LogicLongs = logicLongs.ToArray();
    }

    public int Unknown0 { get; }
    public bool UnknownBoolean0 { get; }
    public LogicLong? UnknownLogicLong { get; }
    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? OptionalValues { get; }
    public ReadOnlyMemory<LogicLong> LogicLongs { get; }

    internal static LogicMapGameChickenTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameChickenTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            LogicMapGameWire.ReadOptionalLogicLong(stream),
            LogicMapGameWire.ReadOptionalDataReferenceVarIntPairs(stream),
            LogicMapGameWire.ReadLogicLongs(stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong);
        LogicMapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
        LogicMapGameWire.WriteLogicLongs(stream, LogicLongs.Span);
    }
}
