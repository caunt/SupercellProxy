using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>One donation to a neighbourhood goods request, with what the donor got for it.</summary>
/// <param name="DonorHomeId">Home of the member who donated.</param>
/// <param name="DonorName">Name of the member who donated.</param>
/// <param name="Amount">How many goods the member gave.</param>
/// <param name="DonatedAt">When the donation was made; its unit is not identified.</param>
/// <param name="DonorLevel">Farm level of the donor.</param>
/// <param name="DonorRole">Neighbourhood role of the donor; the role numbers are not identified.</param>
/// <param name="ExperienceReward">Experience the donor got for the donation.</param>
/// <param name="CoinReward">Coins the donor got for the donation.</param>
/// <param name="ReputationReward">Reputation the donor got for the donation.</param>
/// <param name="Thanked">Whether the requester thanked the donor, as a number; its values are not identified.</param>
public sealed record NeighborhoodRequestDonation(
    LongId DonorHomeId,
    string? DonorName,
    int Amount,
    int DonatedAt,
    int DonorLevel,
    int DonorRole,
    int ExperienceReward,
    int CoinReward,
    int ReputationReward,
    int Thanked
) : ICodecProvider<NeighborhoodRequestDonation>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodRequestDonation> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.DonorHomeId,
        Codecs.OptionalString,
        static value => value.DonorName,
        Codecs.VarInt,
        static value => value.Amount,
        Codecs.VarInt,
        static value => value.DonatedAt,
        Codecs.VarInt,
        static value => value.DonorLevel,
        Codecs.VarInt,
        static value => value.DonorRole,
        Codecs.VarInt,
        static value => value.ExperienceReward,
        Codecs.VarInt,
        static value => value.CoinReward,
        Codecs.VarInt,
        static value => value.ReputationReward,
        Codecs.VarInt,
        static value => value.Thanked,
        static (donorHomeId, donorName, amount, donatedAt, donorLevel, donorRole, experienceReward, coinReward, reputationReward, thanked) => new NeighborhoodRequestDonation(donorHomeId, donorName, amount, donatedAt, donorLevel, donorRole, experienceReward, coinReward, reputationReward, thanked)
    );
}
