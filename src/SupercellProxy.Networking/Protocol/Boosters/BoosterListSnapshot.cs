using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents decoded <c language="csharp">BoosterList</c> home data.</summary>
public sealed record BoosterListSnapshot
{
    /// <summary>Gets or sets the retained boosters.</summary>
    /// <value>The running boosters, each with its own countdown and game mode, in saved order; empty by default.</value>
    public ImmutableArray<BoosterSnapshot> Boosters { get; init; } = [];
}
