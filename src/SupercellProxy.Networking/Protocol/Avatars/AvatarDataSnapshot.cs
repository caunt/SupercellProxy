namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarDataObjects</c> value.
    /// </summary>
    public AvatarDataObjectsSnapshot AvatarDataObjects { get; init; } = new();
}
