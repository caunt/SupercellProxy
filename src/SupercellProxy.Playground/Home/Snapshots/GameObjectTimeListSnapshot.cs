using System.Text.Json;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>GameObjectTimeListSnapshot</c> home data.
/// </summary>
public sealed record GameObjectTimeListSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Action</c> value.
    /// </summary>
    public int Action { get; init; }

    /// <summary>
    /// Gets or sets the <c>List</c> value.
    /// </summary>
    public JsonElement[] List { get; init; } = [];
}
