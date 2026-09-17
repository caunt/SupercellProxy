namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents decoded <c language="csharp">BoosterList</c> home data.</summary>
public sealed record BoosterListSnapshot
{
    /// <summary>Gets the retained boosters.</summary>
    public BoosterSnapshot[] Boosters { get; init; } = [];
}
