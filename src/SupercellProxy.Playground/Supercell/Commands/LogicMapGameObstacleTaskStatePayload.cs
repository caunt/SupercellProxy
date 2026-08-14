using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameObstacleTaskStatePayload : LogicMapGameTaskStatePayload
{
    public LogicMapGameObstacleTaskStatePayload(bool unknownBoolean0, bool unknownBoolean1, LogicLong? unknownLogicLong, ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? optionalValues)
    {
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        UnknownLogicLong = unknownLogicLong;
        OptionalValues = optionalValues?.ToArray();
    }

    public bool UnknownBoolean0 { get; }
    public bool UnknownBoolean1 { get; }
    public LogicLong? UnknownLogicLong { get; }
    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? OptionalValues { get; }

    internal static LogicMapGameObstacleTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameObstacleTaskStatePayload(
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            LogicMapGameWire.ReadOptionalLogicLong(stream),
            LogicMapGameWire.ReadOptionalDataReferenceVarIntPairs(stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong);
        LogicMapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
    }
}
