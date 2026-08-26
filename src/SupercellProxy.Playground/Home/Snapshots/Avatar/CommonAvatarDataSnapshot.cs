using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>CommonAvatarDataSnapshot</c> home data.
/// </summary>
public sealed record CommonAvatarDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c>ChronosEvents</c> value.
    /// </summary>
    public ChronosEventsSnapshot? ChronosEvents { get; init; }

    /// <summary>
    /// Gets or sets the <c>CustomizationManager</c> value.
    /// </summary>
    public CustomizationManagerSnapshot? CustomizationManager { get; init; }

    /// <summary>
    /// Gets or sets the <c>CreatureManager</c> value.
    /// </summary>
    public CreatureManagerSnapshot? CreatureManager { get; init; }

    /// <summary>
    /// Gets or sets the <c>FarmPassManager</c> value.
    /// </summary>
    public FarmPassSnapshot? FarmPassManager { get; init; }

    /// <summary>
    /// Gets or sets the <c>NeighborhoodObjectManager</c> value.
    /// </summary>
    [JsonPropertyName("LogicNeighborhoodObjectManager")]
    public NeighborhoodObjectManagerSnapshot? NeighborhoodObjectManager { get; init; }

    /// <summary>
    /// Gets or sets the <c>MapGameManager</c> value.
    /// </summary>
    public MapGameSnapshot? MapGameManager { get; init; }
}
