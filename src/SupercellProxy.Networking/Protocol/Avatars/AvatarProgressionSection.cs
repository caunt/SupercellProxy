using SupercellProxy.Networking.Protocol.Avatars.Collections;
using SupercellProxy.Networking.Protocol.Avatars.Derby;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>The avatar progression block: derby, live events, map game, age gate, purchases and settings.</summary>
internal readonly record struct AvatarProgressionSection(
    int UnknownNullableListCount,
    LongId? CurrentDerbyInstanceId,
    LongId? PreviousDerbyInstanceId,
    int LeagueType,
    int PreviousLeagueType,
    int LeagueScore,
    AvatarDerbyProgress DerbyProgress,
    EventManagerData EventManager,
    LiveGlobalConfigurations LiveGlobals,
    AvatarDerbyMinigameCounters DerbyMinigameCounters,
    LongId? MapGameId,
    LongId? UnknownOptionalId3,
    int Unknown4,
    MapGameParticipant[]? MapGameParticipants,
    int BirthTimestamp,
    string? AgeCountryCode,
    bool StorePromotionAllowed,
    string? UnknownString1,
    bool UnknownBoolean1,
    PurchaseHistory ShopPurchaseHistory,
    AvatarSettings? Settings
) : ICodecProvider<AvatarProgressionSection>
{
    /// <inheritdoc />
    public static ICodec<AvatarProgressionSection> Codec { get; } = Codecs.Composite(
        Codecs.Map(
            Codecs.VarInt,
            static count => count > 0 ? throw new InvalidDataException(message: "The nullable polymorphic avatar section is not implemented.") : count,
            static count => count > 0 ? throw new InvalidOperationException(message: "Cannot encode an unsupported avatar section.") : count
        ),
        static section => section.UnknownNullableListCount,
        Codecs.OptionalLongId,
        static section => section.CurrentDerbyInstanceId,
        Codecs.OptionalLongId,
        static section => section.PreviousDerbyInstanceId,
        Codecs.VarInt,
        static section => section.LeagueType,
        Codecs.VarInt,
        static section => section.PreviousLeagueType,
        Codecs.VarInt,
        static section => section.LeagueScore,
        AvatarDerbyProgress.Codec,
        static section => section.DerbyProgress,
        EventManagerData.Codec,
        static section => section.EventManager,
        LiveGlobalConfigurations.Codec,
        static section => section.LiveGlobals,
        AvatarDerbyMinigameCounters.Codec,
        static section => section.DerbyMinigameCounters,
        MapGameBlock.Codec,
        static section => new MapGameBlock(section.MapGameId, section.UnknownOptionalId3, section.Unknown4, section.MapGameParticipants),
        AgeBlock.Codec,
        static section => new AgeBlock(section.BirthTimestamp, section.AgeCountryCode, section.StorePromotionAllowed),
        Codecs.OptionalString,
        static section => section.UnknownString1,
        Codecs.Boolean,
        static section => section.UnknownBoolean1,
        PurchaseHistory.Codec,
        static section => section.ShopPurchaseHistory,
        Codecs.Optional(AvatarSettings.Codec),
        static section => section.Settings,
        static (
            unknownNullableListCount,
            currentDerbyInstanceId,
            previousDerbyInstanceId,
            leagueType,
            previousLeagueType,
            leagueScore,
            derbyProgress,
            eventManager,
            liveGlobals,
            derbyMinigameCounters,
            mapGame,
            age,
            unknownString1,
            unknownBoolean1,
            shopPurchaseHistory,
            settings
        ) => new AvatarProgressionSection(
            unknownNullableListCount,
            currentDerbyInstanceId,
            previousDerbyInstanceId,
            leagueType,
            previousLeagueType,
            leagueScore,
            derbyProgress,
            eventManager,
            liveGlobals,
            derbyMinigameCounters,
            mapGame.MapGameId,
            mapGame.UnknownOptionalId3,
            mapGame.Unknown4,
            mapGame.Participants,
            age.BirthTimestamp,
            age.AgeCountryCode,
            age.StorePromotionAllowed,
            unknownString1,
            unknownBoolean1,
            shopPurchaseHistory,
            settings
        )
    );

    // The age gate fields, read as one group to keep the field list within a composite.
    private readonly record struct AgeBlock(int BirthTimestamp, string? AgeCountryCode, bool StorePromotionAllowed)
    {
        public static ICodec<AgeBlock> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static block => block.BirthTimestamp,
            Codecs.OptionalString,
            static block => block.AgeCountryCode,
            Codecs.Boolean,
            static block => block.StorePromotionAllowed,
            static (birthTimestamp, ageCountryCode, storePromotionAllowed) => new AgeBlock(birthTimestamp, ageCountryCode, storePromotionAllowed)
        );
    }

    // Native reads the map game participants only when the avatar has a map game.
    private readonly record struct MapGameBlock(LongId? MapGameId, LongId? UnknownOptionalId3, int Unknown4, MapGameParticipant[]? Participants)
    {
        public static ICodec<MapGameBlock> Codec { get; } = Codecs.Create(
            static stream =>
            {
                LongId? mapGameId = stream.ReadOptionalLongId();
                LongId? unknownOptionalId3 = stream.ReadOptionalLongId();
                int unknown4 = stream.ReadVarInt();

                return new MapGameBlock(mapGameId, unknownOptionalId3, unknown4, mapGameId is null ? null : ParticipantsCodec.Read(stream));
            },
            static (stream, block) =>
            {
                stream.WriteOptionalLongId(block.MapGameId);
                stream.WriteOptionalLongId(block.UnknownOptionalId3);
                stream.WriteVarInt(block.Unknown4);

                if (block.MapGameId is not null)
                    ParticipantsCodec.Write(stream, block.Participants);
            }
        );

        private static ICodec<MapGameParticipant[]?> ParticipantsCodec => field ??= Codecs.Optional(Codecs.Array(MapGameParticipant.Codec, CollectionCount.Bounded));
    }
}
