using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native three-value structure embedded in a map-game pawn. Its semantic field names are not present in the stripped client.
/// </summary>
public sealed record LogicMapGamePawnNestedData(int Unknown0, int Unknown1, int Unknown2)
{
    internal static LogicMapGamePawnNestedData Decode(SupercellStream stream)
    {
        return new LogicMapGamePawnNestedData(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
