using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>One neighborhood request donation entry.</summary>
/// <param name="DonorHomeId">The donor home id.</param>
/// <param name="DonorName">The donor name.</param>
/// <param name="Amount">The amount.</param>
/// <param name="DonatedAt">The donated at.</param>
/// <param name="DonorLevel">The donor level.</param>
/// <param name="DonorRole">The donor role.</param>
/// <param name="ExperienceReward">The experience reward.</param>
/// <param name="CoinReward">The coin reward.</param>
/// <param name="ReputationReward">The reputation reward.</param>
/// <param name="Thanked">The thanked.</param>
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
