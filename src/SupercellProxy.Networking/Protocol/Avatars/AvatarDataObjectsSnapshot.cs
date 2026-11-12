namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">AvatarDataObjectsSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataObjectsSnapshot
{
    /// <summary>Gets or sets the avatar-owned farm object metadata.</summary>
    /// <value>
    /// One entry per avatar-tracked object of the farm layout; the entries of each data id correspond, in order, to the layout's
    /// objects of that id. Empty when the document lists none.
    /// </value>
    public FarmObjectSnapshot[] Farm { get; init; } = [];

    /// <summary>Gets or sets the avatar-owned town object metadata.</summary>
    /// <value>
    /// The town's entries; empty when the document lists none.
    /// </value>
    public FarmObjectSnapshot[] Town { get; init; } = [];

    /// <summary>Gets or sets the avatar-owned decoration-canvas object metadata.</summary>
    /// <value>
    /// The decoration canvas's entries; empty when the document lists none.
    /// </value>
    public FarmObjectSnapshot[] Decoration { get; init; } = [];

    /// <summary>Gets or sets the avatar-owned fishing-area object metadata.</summary>
    /// <value>
    /// The fishing lake's entries; empty when the document lists none.
    /// </value>
    public FarmObjectSnapshot[] Fishing { get; init; } = [];

    /// <summary>
    /// Gets or sets the saved state of the account-wide game systems that are not tied to one area: boat and truck tracks, live
    /// events, Farm Pass, tutorials, the newspaper and the like.
    /// </summary>
    /// <value>
    /// One snapshot per native manager the document saves; each manager the document leaves out is <see langword="null"/>.
    /// </value>
    public CommonAvatarDataSnapshot Common { get; init; } = new();
}
