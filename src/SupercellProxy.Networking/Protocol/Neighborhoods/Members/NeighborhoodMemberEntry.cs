using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>One member returned by a neighborhood member-list response.</summary>
public sealed record NeighborhoodMemberEntry(
    LongIdentifier HomeIdentifier,
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
            stream.ReadLongIdentifier(),
            stream.ReadString(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt()
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongIdentifier(HomeIdentifier);
        stream.WriteString(FarmName);
        stream.WriteVariableInt(Level);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        stream.WriteVariableInt(Unknown3);
        stream.WriteVariableInt(Unknown4);
        stream.WriteVariableInt(Unknown5);
        stream.WriteVariableInt(Unknown6);
    }
}
