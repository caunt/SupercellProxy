namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataObjectsSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataObjectsSnapshot
{
    /// <summary>Gets the avatar-owned farm object metadata.</summary>
    public FarmObjectSnapshot[] Farm { get; init; } = [];

    /// <summary>Gets the avatar-owned town object metadata.</summary>
    public FarmObjectSnapshot[] Town { get; init; } = [];

    /// <summary>Gets the avatar-owned decoration-canvas object metadata.</summary>
    public FarmObjectSnapshot[] Decoration { get; init; } = [];

    /// <summary>Gets the avatar-owned fishing-area object metadata.</summary>
    public FarmObjectSnapshot[] Fishing { get; init; } = [];

    /// <summary>
    /// Saved state of the account-wide game systems that are not tied to one area: boat and truck tracks, live events, Farm Pass,
    /// tutorials, the newspaper and the like.
    /// </summary>
    public CommonAvatarDataSnapshot Common { get; init; } = new();
}
