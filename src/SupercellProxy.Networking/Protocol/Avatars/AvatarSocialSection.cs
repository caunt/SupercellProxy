using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Town;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>The avatar social block: roadside shop, neighborhood, mail and help entries.</summary>
internal readonly record struct AvatarSocialSection(
    RoadsideShopEntry[] RoadsideShop,
    NeighborhoodData? Neighborhood,
    MailEntry[] MailEntries,
    int[] UnknownValues0,
    BoatCrateHelpEntry[] BoatCrateHelpEntries,
    bool TrainStationReady,
    bool IsMuted,
    bool CanEditFarm,
    PassengerReservation[] PassengerReservations,
    PickedPassenger[] PickedPassengers,
    ServiceHelpRequest[] ServiceHelpRequests,
    ServiceHelpRequest[] UnknownEntries3
) : ICodecProvider<AvatarSocialSection>
{
    /// <inheritdoc />
    public static ICodec<AvatarSocialSection> Codec { get; } = Codecs.Composite(
        Codecs.Array(RoadsideShopEntry.Codec, CollectionCount.Bounded),
        static section => section.RoadsideShop,
        Codecs.Optional(NeighborhoodData.Codec),
        static section => section.Neighborhood,
        Codecs.Array(MailEntry.Codec, CollectionCount.Bounded),
        static section => section.MailEntries,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static section => section.UnknownValues0,
        Codecs.Array(BoatCrateHelpEntry.Codec, CollectionCount.Bounded),
        static section => section.BoatCrateHelpEntries,
        Codecs.Boolean,
        static section => section.TrainStationReady,
        Codecs.Boolean,
        static section => section.IsMuted,
        Codecs.Boolean,
        static section => section.CanEditFarm,
        Codecs.Array(PassengerReservation.Codec, CollectionCount.Bounded),
        static section => section.PassengerReservations,
        Codecs.Array(PickedPassenger.Codec, CollectionCount.Bounded),
        static section => section.PickedPassengers,
        Codecs.Array(ServiceHelpRequest.Codec, CollectionCount.Bounded),
        static section => section.ServiceHelpRequests,
        Codecs.Array(ServiceHelpRequest.Codec, CollectionCount.Bounded),
        static section => section.UnknownEntries3,
        static (
            roadsideShop,
            neighborhood,
            mailEntries,
            unknownValues0,
            boatCrateHelpEntries,
            trainStationReady,
            isMuted,
            canEditFarm,
            passengerReservations,
            pickedPassengers,
            serviceHelpRequests,
            unknownEntries3
        ) => new AvatarSocialSection(
            roadsideShop,
            neighborhood,
            mailEntries,
            unknownValues0,
            boatCrateHelpEntries,
            trainStationReady,
            isMuted,
            canEditFarm,
            passengerReservations,
            pickedPassengers,
            serviceHelpRequests,
            unknownEntries3
        )
    );
}
