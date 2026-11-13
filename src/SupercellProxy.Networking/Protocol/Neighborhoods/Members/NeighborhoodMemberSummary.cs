using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>One member returned by a neighborhood member-list response.</summary>
/// <param name="HomeId">Id of the member's home (account), the first field of the entry.</param>
/// <param name="FarmName">The member's farm name; written as a required string, so it is never <see langword="null"/>.</param>
/// <param name="Level">The member's farm experience level.</param>
/// <param name="Unknown0">First of seven VarInts that follow the level; its meaning is not identified.</param>
/// <param name="Unknown1">Second VarInt after the level; its meaning is not identified.</param>
/// <param name="Unknown2">Third VarInt after the level; its meaning is not identified.</param>
/// <param name="Unknown3">Fourth VarInt after the level; its meaning is not identified.</param>
/// <param name="Unknown4">Fifth VarInt after the level; its meaning is not identified.</param>
/// <param name="Unknown5">Sixth VarInt after the level; its meaning is not identified.</param>
/// <param name="Unknown6">Seventh and last VarInt of the entry; its meaning is not identified.</param>
public sealed record NeighborhoodMemberSummary(LongId HomeId,
    string FarmName,
    int Level,
    int Unknown0,
    int Unknown1,
    int Unknown2,
    int Unknown3,
    int Unknown4,
    int Unknown5,
    int Unknown6) : ICodecProvider<NeighborhoodMemberSummary>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMemberSummary> Codec { get; } = Codecs.Composite(Codecs.LongId,
        static value => value.HomeId,
        Codecs.Utf8String,
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
        static (homeId, farmName, level, unknown0, unknown1, unknown2, unknown3, unknown4, unknown5, unknown6) => new NeighborhoodMemberSummary(homeId, farmName, level, unknown0, unknown1, unknown2, unknown3, unknown4, unknown5, unknown6));
}
