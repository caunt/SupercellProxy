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

    /// <summary>Gets the avatar-owned town object metadata.</summary>
    public FarmObjectSnapshot[] Town { get; init; } = [];

    /// <summary>Gets the avatar-owned decoration-canvas object metadata.</summary>
    public FarmObjectSnapshot[] Decoration { get; init; } = [];

    /// <summary>Gets the avatar-owned fishing-area object metadata.</summary>
    public FarmObjectSnapshot[] Fishing { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Common</c> value.
    /// </summary>
    public CommonAvatarDataSnapshot Common { get; init; } = new();
}
