using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">CommonAvatarDataSnapshot</c> home data.
/// </summary>
internal sealed record CommonAvatarDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">ChronosEvents</c> value.
    /// </summary>
    public ChronosEventsSnapshot? ChronosEvents { get; init; }

    /// <summary>
    /// <para>Gets the retained decoration-event manager state.</para>
    /// </summary>
    [JsonPropertyName("DecoEventMgr")]
    public DecorationEventManagerSnapshot? DecorationEventManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CustomizationManager</c> value.
    /// </summary>
    public CustomizationManagerSnapshot? CustomizationManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CreatureManager</c> value.
    /// </summary>
    public CreatureManagerSnapshot? CreatureManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmPassManager</c> value.
    /// </summary>
    public FarmPassSnapshot? FarmPassManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">NeighborhoodObjectManager</c> value.
    /// </summary>
    [JsonPropertyName("LogicNeighborhoodObjectManager")]
    public NeighborhoodObjectManagerSnapshot? NeighborhoodObjectManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGameManager</c> value.
    /// </summary>
    public MapGameSnapshot? MapGameManager { get; init; }
}
