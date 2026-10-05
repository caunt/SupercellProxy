using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Contains a placed net or trap and its retained native drop selection.</summary>
public sealed record FishingNetSnapshot : ExtensibleDocument
{
    /// <summary>Gets the data id of the net or trap.</summary>
    public int Data { get; init; }
    /// <summary>Gets the selected drop data ids.</summary>
    public int[] Drops { get; init; } = [];
    /// <summary>Gets the independent drop generator's retained state.</summary>
    public int RandomSeed { get; init; }
}
