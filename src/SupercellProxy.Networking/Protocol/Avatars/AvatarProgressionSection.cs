using SupercellProxy.Networking.Protocol.Avatars.Collections;
using SupercellProxy.Networking.Protocol.MapGame;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Named result returned by DecodeProgressionState.</summary>
internal readonly record struct AvatarProgressionSection(
    int UnknownNullableListCount,
    LongId? UnknownOptionalId0,
    LongId? UnknownOptionalId1,
    int LeagueType,
    int UnknownLeagueValue,
    int LeagueScore,
    int[] UnknownValues1,
    AvatarCollectionSection UnknownManager0,
    AvatarStringSection UnknownManager1,
    int[] UnknownValues2,
    LongId? MapGameId,
    LongId? UnknownOptionalId3,
    int Unknown4,
    MapGameParticipant[]? MapGameParticipants,
    int BirthTimestamp,
    string? AgeCountryCode,
    bool StorePromotionAllowed,
    string? UnknownString1,
    bool UnknownBoolean1,
    AvatarStateSection UnknownManager2,
    AvatarSettings? Settings
);
