namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataSnapshot</c> home data.
/// </summary>
internal sealed record AvatarDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarDataObjects</c> value.
    /// </summary>
    public AvatarDataObjectsSnapshot AvatarDataObjects { get; init; } = new();
}
