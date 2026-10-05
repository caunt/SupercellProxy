using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.Town;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Named result returned by DecodeSocialState.</summary>
internal readonly record struct AvatarSocialSection(
    RoadsideShopEntry[] RoadsideShop,
    NeighborhoodData? Neighborhood,
    MailEntry[] MailEntries,
    int[] UnknownValues0,
    BoatCrateHelpEntry[] BoatCrateHelpEntries,
    bool TrainStationReady,
    bool IsMuted,
    bool CanEditFarm,
    AvatarIdFlagEntry[] UnknownEntries1,
    PickedPassenger[] PickedPassengers,
    AvatarIdPairEntry[] UnknownEntries2,
    AvatarIdPairEntry[] UnknownEntries3
);
