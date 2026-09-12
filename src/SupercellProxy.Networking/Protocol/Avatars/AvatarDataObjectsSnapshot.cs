namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataObjectsSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataObjectsSnapshot
{
    /// <summary>
    /// Gets the Farm value.
    /// </summary>
    public FarmObjectSnapshot[] Farm { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Common</c> value.
    /// </summary>
    public CommonAvatarDataSnapshot Common { get; init; } = new();
}
