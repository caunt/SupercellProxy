using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

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
public sealed record PlaceFishingBaitCommand(int FishingSpotGlobalId, int BiteDelayMilliseconds, int BaitGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.PlaceFishingBaitCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static PlaceFishingBaitCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishingSpotGlobalId = stream.ReadVarInt();
        int biteDelayMilliseconds = stream.ReadVarInt();
        int baitGlobalId = stream.ReadVarInt();

        return new PlaceFishingBaitCommand(fishingSpotGlobalId, biteDelayMilliseconds, baitGlobalId);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FishingSpotGlobalId);
        stream.WriteVarInt(BiteDelayMilliseconds);
        stream.WriteVarInt(BaitGlobalId);
    }
}
