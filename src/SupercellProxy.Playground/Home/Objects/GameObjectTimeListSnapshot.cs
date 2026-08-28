using System.Text.Json;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">GameObjectTimeListSnapshot</c> home data.
/// </summary>
internal sealed record GameObjectTimeListSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Action</c> value.
    /// </summary>
    public int Action { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">List</c> value.
    /// </summary>
    public JsonElement[] List { get; init; } = [];
}
