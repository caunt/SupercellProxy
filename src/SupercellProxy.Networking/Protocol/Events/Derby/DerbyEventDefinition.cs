using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes the server-supplied rules and presentation of native event type zero.</summary>
public sealed record DerbyEventDefinition
{
    /// <summary>Identifies Derby in the native event factory.</summary>
    public const int EventType = 0;

    /// <summary>Gets the rules applied to every league instead of individual league sections.</summary>
    [JsonPropertyName("AllLeagues")]
    public DerbyLeagueDefinition? AllLeagues { get; init; }

    /// <summary>Gets whether neighborhood members can discard tasks on the shared board.</summary>
    [JsonPropertyName("boardTaskDiscardingAllowed")]
    public bool BoardTaskDiscardAllowed { get; init; } = true;

    /// <summary>Gets the champions league overrides.</summary>
    [JsonPropertyName("ChampionsLeague")]
    public DerbyLeagueDefinition? ChampionsLeague { get; init; }

    /// <summary>Gets the expert league overrides.</summary>
    [JsonPropertyName("ExpertLeague")]
    public DerbyLeagueDefinition? ExpertLeague { get; init; }

    /// <summary>Gets the downloadable header image.</summary>
    [JsonPropertyName("headerImage")]
    public EventAssetDefinition? HeaderImage { get; init; }

    /// <summary>Gets the downloadable localization file.</summary>
    [JsonPropertyName("localizationFile")]
    public EventAssetDefinition? LocalizationFile { get; init; }

    /// <summary>Gets the downloadable logo image.</summary>
    [JsonPropertyName("logoImage")]
    public EventAssetDefinition? LogoImage { get; init; }

    /// <summary>Gets the maximum number of rewarded bingo lines.</summary>
    [JsonPropertyName("maxBingoLines")]
    public int MaximumBingoLines { get; init; }

    /// <summary>Gets the novice league overrides.</summary>
    [JsonPropertyName("NoviceLeague")]
    public DerbyLeagueDefinition? NoviceLeague { get; init; }

    /// <summary>Gets whether players can discard their own active task.</summary>
    [JsonPropertyName("ownTaskDiscardingAllowed")]
    public bool OwnTaskDiscardAllowed { get; init; } = true;

    /// <summary>Gets the professional league overrides.</summary>
    [JsonPropertyName("ProfessionalLeague")]
    public DerbyLeagueDefinition? ProfessionalLeague { get; init; }

    /// <summary>Gets the ordered, league-specific placement and threshold reward overrides.</summary>
    [JsonPropertyName("DerbyRewards")]
    public DerbyRewardDefinition?[]? Rewards { get; init; }

    /// <summary>Gets the rookie league overrides.</summary>
    [JsonPropertyName("RookieLeague")]
    public DerbyLeagueDefinition? RookieLeague { get; init; }

    /// <summary>Gets the event's explanation of its special rules.</summary>
    [JsonPropertyName("specialRulesText")]
    public string? SpecialRulesText { get; init; }

    /// <summary>Gets the event-start notification text.</summary>
    [JsonPropertyName("startNotification")]
    public string? StartNotification { get; init; }

    /// <summary>Gets the price to bypass a task cooldown; zero retains the ordinary rule.</summary>
    [JsonPropertyName("taskCooldownCost")]
    public int TaskCooldownPrice { get; init; }

    /// <summary>Gets the task cooldown in seconds; zero retains the ordinary rule.</summary>
    [JsonPropertyName("taskCooldownTime")]
    public int TaskCooldownSeconds { get; init; }

    /// <summary>Gets the event title.</summary>
    [JsonPropertyName("titleText")]
    public string? Title { get; init; }

    /// <summary>Gets the native title style name.</summary>
    [JsonPropertyName("titleStyle")]
    public string? TitleStyle { get; init; }

    /// <summary>Gets the shared event-board presentation.</summary>
    [JsonPropertyName("UI")]
    public EventUserInterfaceDefinition? UserInterface { get; init; }
}
