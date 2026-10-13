using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>
/// Places the selected bait on one fishing-area spot.
/// The native execute resolves the spot in table 96 and the bait in table 98, rejects a negative
/// <see cref="BiteDelayMilliseconds"/>, moves the spot's attached fish to the baited state, and
/// hands the delay to the spot's bait setter together with the resolved bait row.
/// </summary>
/// <param name="FishingSpotGlobalId">The baited spot's table-96 game-object global id.</param>
/// <param name="BiteDelayMilliseconds">
/// The client-chosen delay before a fish bites. The native bait setter converts it into bite-timer
/// updates as <c language="csharp">value * 30 / 1000</c>; only non-negative values are accepted.
/// </param>
/// <param name="BaitGlobalId">The bait's data row in the baits table.</param>
public sealed record PlaceFishingBaitCommand(int FishingSpotGlobalId, int BiteDelayMilliseconds, int BaitGlobalId) : CodecCommand<PlaceFishingBaitCommand>, ICommandCodecProvider<PlaceFishingBaitCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceFishingBaitCommand> Codec { get; } = Codecs.Command(
        type: 111,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FishingSpotGlobalId,
            Codecs.VarInt,
            static command => command.BiteDelayMilliseconds,
            Codecs.VarInt,
            static command => command.BaitGlobalId,
            static (fishingSpotGlobalId, biteDelayMilliseconds, baitGlobalId) => new PlaceFishingBaitCommand(fishingSpotGlobalId, biteDelayMilliseconds, baitGlobalId)
        )
    );
}
