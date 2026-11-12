using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Contains a placed net or trap and its retained native drop selection.</summary>
public sealed record FishingNetSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the data id of the net or trap.</summary>
    /// <value>Global id of the placed fishing net or lobster or duck trap row.</value>
    public int Data { get; init; }
    /// <summary>Gets or sets the selected drop data ids.</summary>
    /// <value>Global ids of the goods, lobsters or ducks the net holds, one entry per drop and repeats allowed.</value>
    public int[] Drops { get; init; } = [];
    /// <summary>Gets or sets the independent drop generator's retained state.</summary>
    /// <value>The saved state of the net's own random stream, separate from the home random; 0 when never saved.</value>
    public int RandomSeed { get; init; }
}
