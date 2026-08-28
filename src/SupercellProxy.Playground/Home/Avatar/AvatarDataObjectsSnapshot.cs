namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataObjectsSnapshot</c> home data.
/// </summary>
internal sealed record AvatarDataObjectsSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Common</c> value.
    /// </summary>
    public CommonAvatarDataSnapshot Common { get; init; } = new();
}
