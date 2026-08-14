using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameGasStationTaskStatePayload : LogicMapGameTaskStatePayload
{
    public LogicMapGameGasStationTaskStatePayload(bool unknownBoolean0, bool unknownBoolean1, int unknown0, ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? optionalValues)
    {
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        Unknown0 = unknown0;
        OptionalValues = optionalValues?.ToArray();
    }

    public bool UnknownBoolean0 { get; }
    public bool UnknownBoolean1 { get; }
    public int Unknown0 { get; }
    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? OptionalValues { get; }

    internal static LogicMapGameGasStationTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameGasStationTaskStatePayload(
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            LogicMapGameWire.ReadOptionalDataReferenceVarIntPairs(stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteVarInt(Unknown0);
        LogicMapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
    }
}
