using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>One member returned by a neighborhood member-list response.</summary>
public sealed record NeighborhoodMemberSummary(
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
) : ICodecProvider<NeighborhoodMemberSummary>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMemberSummary> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.HomeId,
        Codecs.String,
        static value => value.FarmName,
        Codecs.VarInt,
        static value => value.Level,
        Codecs.VarInt,
        static value => value.Unknown0,
        Codecs.VarInt,
        static value => value.Unknown1,
        Codecs.VarInt,
        static value => value.Unknown2,
        Codecs.VarInt,
        static value => value.Unknown3,
        Codecs.VarInt,
        static value => value.Unknown4,
        Codecs.VarInt,
        static value => value.Unknown5,
        Codecs.VarInt,
        static value => value.Unknown6,
        static (homeId, farmName, level, unknown0, unknown1, unknown2, unknown3, unknown4, unknown5, unknown6) => new NeighborhoodMemberSummary(homeId, farmName, level, unknown0, unknown1, unknown2, unknown3, unknown4, unknown5, unknown6)
    );
}
