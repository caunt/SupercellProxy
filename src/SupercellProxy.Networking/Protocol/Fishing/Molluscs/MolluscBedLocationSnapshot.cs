namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>One location of a mussel bed, as saved with the bed.</summary>
/// <param name="Unlocked">Whether the location is unlocked.</param>
/// <param name="MolluscGlobalId">The object id of the mussel growing there, or 0 when none was linked.</param>
public sealed record MolluscBedLocationSnapshot(bool Unlocked, int MolluscGlobalId);
