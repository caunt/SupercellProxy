namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataSnapshot
{
    /// <summary>
    /// The avatar's per-area object metadata (farm, town, decoration canvas, fishing) and its account-wide system state.
    /// </summary>
    public AvatarDataObjectsSnapshot AvatarDataObjects { get; init; } = new();
}
