using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>One member returned by a neighborhood member-list response.</summary>
public sealed record NeighborhoodMemberEntry(
    LongId HomeId,
    string FarmName,
    int Level,
    int Unknown0,
    int Unknown1,
    int Unknown2,
    int Unknown3,
    int Unknown4,
    int Unknown5,
    int Unknown6
)
{
    internal static NeighborhoodMemberEntry Decode(MessageStream stream)
    {
        return new NeighborhoodMemberEntry(
            stream.ReadLongId(),
            stream.ReadString(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongId(HomeId);
        stream.WriteString(FarmName);
        stream.WriteVarInt(Level);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteVarInt(Unknown6);
    }
}
