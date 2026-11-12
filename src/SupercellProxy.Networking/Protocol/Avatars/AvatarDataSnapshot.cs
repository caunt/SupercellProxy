namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataSnapshot
{
    /// <summary>
    /// Gets or sets the avatar's per-area object metadata (farm, town, decoration canvas, fishing) and its account-wide system state.
    /// </summary>
    /// <value>
    /// Read from the <c>AvatarDataObjects</c> object of the decompressed avatar data JSON; empty lists and no saved managers when the
    /// home message carried no avatar data document.
    /// </value>
    public AvatarDataObjectsSnapshot AvatarDataObjects { get; init; } = new();
}
