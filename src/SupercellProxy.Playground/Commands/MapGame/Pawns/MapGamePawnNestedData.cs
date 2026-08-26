using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native three-value structure embedded in a map-game pawn. Its semantic field names are not present in the stripped client.</para>
/// </summary>
public sealed record MapGamePawnNestedData(int Unknown0, int Unknown1, int Unknown2)
{
    internal static MapGamePawnNestedData Decode(MessageStream stream)
    {
        return new MapGamePawnNestedData(
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
    }
}
