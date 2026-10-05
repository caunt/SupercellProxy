using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>
/// Completes a hooked catch on one fishing-area spot. The native execute accepts the command only in
/// the fishing game mode, resolves the spot in table 96, the fish in table 97 and the bait in table
/// 98, and then takes one of two branches: <see cref="Caught"/> keeps the catch, while a cleared
/// <see cref="Caught"/> releases it and returns every attached fish to the swimming state.
/// </summary>
/// <param name="Caught">
/// Whether the hooked catch is kept. The native execute requires both a resolved fish and a resolved
/// catch reference while it is set, and takes the releasing branch while it is cleared.
/// </param>
/// <param name="FishingSpotGlobalId">The spot's table-96 game-object global id.</param>
/// <param name="FishGlobalId">The caught fish's table-97 game-object global id.</param>
/// <param name="BaitGlobalId">The placed bait's data row in the baits table.</param>
public sealed record CompleteFishingCatchCommand(bool Caught, int FishingSpotGlobalId, int FishGlobalId, int BaitGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CompleteFishingCatchCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CompleteFishingCatchCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool caught = stream.ReadBoolean();
        int fishingSpotGlobalId = stream.ReadVarInt();
        int fishGlobalId = stream.ReadVarInt();
        int baitGlobalId = stream.ReadVarInt();

        return new CompleteFishingCatchCommand(caught, fishingSpotGlobalId, fishGlobalId, baitGlobalId);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(Caught);
        stream.WriteVarInt(FishingSpotGlobalId);
        stream.WriteVarInt(FishGlobalId);
        stream.WriteVarInt(BaitGlobalId);
    }
}
