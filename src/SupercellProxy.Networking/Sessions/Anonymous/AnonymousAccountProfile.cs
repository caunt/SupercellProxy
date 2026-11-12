namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>The most recently observed public farm information for an anonymous account.</summary>
/// <param name="Name">Farm name shown to other players, or <see langword="null"/> while the farm has not been named.</param>
/// <param name="Level">Experience level read from the avatar's <c>ExpLevel</c> resource.</param>
/// <param name="Experience">Experience points held in the avatar's <c>ExpPoints</c> resource; 0 when the avatar does not report them.</param>
public sealed record AnonymousAccountProfile(string? Name, int Level, int Experience)
{
    /// <summary>Gets or sets the most recently observed coin balance.</summary>
    /// <value>Amount of the <c>Cash</c> resource in the avatar's inventory, or <see langword="null"/> when it was not reported.</value>
    public int? Coins { get; init; }
    /// <summary>Gets or sets the most recently observed diamond balance.</summary>
    /// <value>Amount of the <c>Diamonds</c> resource in the avatar's inventory, or <see langword="null"/> when it was not reported.</value>
    public int? Diamonds { get; init; }
    /// <summary>Gets or sets the farm identity reported by the server.</summary>
    /// <value>The avatar's home id as one 64-bit number, or <see langword="null"/> when unknown, in which case the account id stands in for it.</value>
    public long? HomeId { get; init; }
}
