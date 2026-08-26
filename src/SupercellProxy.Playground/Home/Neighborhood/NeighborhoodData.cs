using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>NeighborhoodData</c>.
/// </summary>
/// <param name="NeighborhoodId">The <c>NeighborhoodId</c> value.</param>
/// <param name="NeighborhoodName">The <c>NeighborhoodName</c> value.</param>
/// <param name="NeighborhoodRole">The <c>NeighborhoodRole</c> value.</param>
/// <param name="BadgeUnknown0">The <c>BadgeUnknown0</c> value.</param>
/// <param name="BadgeUnknown1">The <c>BadgeUnknown1</c> value.</param>
/// <param name="BadgeUnknown2">The <c>BadgeUnknown2</c> value.</param>
/// <param name="Unknown0">The <c>Unknown0</c> value.</param>
/// <param name="Unknown1">The <c>Unknown1</c> value.</param>
/// <param name="Unknown2">The <c>Unknown2</c> value.</param>
public record NeighborhoodData(
    LongId NeighborhoodId,
    string? NeighborhoodName,
    int NeighborhoodRole,
    int BadgeUnknown0,
    int BadgeUnknown1,
    int BadgeUnknown2,
    int Unknown0,
    int Unknown1,
    int Unknown2
)
{
    internal static NeighborhoodData Decode(MessageStream stream) =>
        new(
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteOptionalString(NeighborhoodName);
        stream.WriteVarInt(NeighborhoodRole);
        stream.WriteVarInt(BadgeUnknown0);
        stream.WriteVarInt(BadgeUnknown1);
        stream.WriteVarInt(BadgeUnknown2);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
