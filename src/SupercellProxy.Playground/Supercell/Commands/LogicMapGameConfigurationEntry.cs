using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native optional logic-long and three-value map-game configuration entry.
/// </summary>
public sealed record LogicMapGameConfigurationEntry(LogicLong? UnknownLogicLong, int Unknown0, int Unknown1, int Unknown2)
{
    internal static LogicMapGameConfigurationEntry Decode(SupercellStream stream)
    {
        return new LogicMapGameConfigurationEntry(
            LogicMapGameWire.ReadOptionalLogicLong(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
