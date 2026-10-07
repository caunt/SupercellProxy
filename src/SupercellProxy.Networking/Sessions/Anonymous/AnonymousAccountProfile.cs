namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>The most recently observed public farm information for an anonymous account.</summary>
public sealed record AnonymousAccountProfile(string? Name, int Level, int Experience)
{
    /// <summary>Gets the most recently observed coin balance.</summary>
    public int? Coins { get; init; }
    /// <summary>Gets the most recently observed diamond balance.</summary>
    public int? Diamonds { get; init; }
    /// <summary>Gets the farm identity reported by the server.</summary>
    public long? HomeId { get; init; }
}
