using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>AvatarDataSnapshot</c> home data.
/// </summary>
public sealed record AvatarDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c>AvatarDataObjects</c> value.
    /// </summary>
    public AvatarDataObjectsSnapshot AvatarDataObjects { get; init; } = new();
}
