namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>AvatarDataObjectsSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataObjectsSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Common</c> value.
    /// </summary>
    public CommonAvatarDataSnapshot Common { get; init; } = new();
}
