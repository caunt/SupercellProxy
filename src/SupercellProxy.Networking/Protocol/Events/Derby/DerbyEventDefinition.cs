using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes the server-supplied rules and presentation of native event type zero.</summary>
public sealed record DerbyEventDefinition
{
    /// <summary>Identifies Derby in the native event factory.</summary>
    public const int EventType = 0;

    /// <summary>Gets or sets the rules applied to every league instead of individual league sections.</summary>
    /// <value>One set of league rules for all five leagues, or <see langword="null"/> when the event gives each league its own section.</value>
    [JsonPropertyName("AllLeagues")]
    public DerbyLeagueDefinition? AllLeagues { get; init; }

    /// <summary>Gets or sets whether neighborhood members can discard tasks on the shared board.</summary>
    /// <value><see langword="true"/> if board tasks may be discarded, the default when the definition omits it; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("boardTaskDiscardingAllowed")]
    public bool BoardTaskDiscardAllowed { get; init; } = true;

    /// <summary>Gets or sets the champions league overrides.</summary>
    /// <value>The rules for the fifth league (index 4), or <see langword="null"/> when the event omits the section.</value>
    [JsonPropertyName("ChampionsLeague")]
    public DerbyLeagueDefinition? ChampionsLeague { get; init; }

    /// <summary>Gets or sets the expert league overrides.</summary>
    /// <value>The rules for the fourth league (index 3), or <see langword="null"/> when the event omits the section.</value>
    [JsonPropertyName("ExpertLeague")]
    public DerbyLeagueDefinition? ExpertLeague { get; init; }

    /// <summary>Gets or sets the downloadable header image.</summary>
    /// <value>The asset the client downloads for the derby's header, or <see langword="null"/> when the event uses the default header.</value>
    [JsonPropertyName("headerImage")]
    public EventAssetDefinition? HeaderImage { get; init; }

    /// <summary>Gets or sets the downloadable localization file.</summary>
    /// <value>The asset holding the event's translated texts, or <see langword="null"/> when the event ships none.</value>
    [JsonPropertyName("localizationFile")]
    public EventAssetDefinition? LocalizationFile { get; init; }

    /// <summary>Gets or sets the downloadable logo image.</summary>
    /// <value>The asset the client downloads for the derby's logo, or <see langword="null"/> when the event uses the default logo.</value>
    [JsonPropertyName("logoImage")]
    public EventAssetDefinition? LogoImage { get; init; }

    /// <summary>Gets or sets the maximum number of rewarded bingo lines.</summary>
    /// <value>Bingo lines that can earn rewards or points (saved as <c>maxBingoLines</c>).</value>
    [JsonPropertyName("maxBingoLines")]
    public int MaximumBingoLines { get; init; }

    /// <summary>Gets or sets the novice league overrides.</summary>
    /// <value>The rules for the second league (index 1), or <see langword="null"/> when the event omits the section.</value>
    [JsonPropertyName("NoviceLeague")]
    public DerbyLeagueDefinition? NoviceLeague { get; init; }

    /// <summary>Gets or sets whether players can discard their own active task.</summary>
    /// <value><see langword="true"/> if a player may discard the task they hold, the default when the definition omits it; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("ownTaskDiscardingAllowed")]
    public bool OwnTaskDiscardAllowed { get; init; } = true;

    /// <summary>Gets or sets the professional league overrides.</summary>
    /// <value>The rules for the third league (index 2), or <see langword="null"/> when the event omits the section.</value>
    [JsonPropertyName("ProfessionalLeague")]
    public DerbyLeagueDefinition? ProfessionalLeague { get; init; }

    /// <summary>Gets or sets the ordered, league-specific placement and threshold reward overrides.</summary>
    /// <value>Reward overrides saved as <c>DerbyRewards</c>, each naming a league and a reward category; individual entries may be <see langword="null"/>, and <see langword="null"/> means no overrides.</value>
    [JsonPropertyName("DerbyRewards")]
    public ImmutableArray<DerbyRewardDefinition?>? Rewards { get; init; }

    /// <summary>Gets or sets the rookie league overrides.</summary>
    /// <value>The rules for the first league (index 0), or <see langword="null"/> when the event omits the section.</value>
    [JsonPropertyName("RookieLeague")]
    public DerbyLeagueDefinition? RookieLeague { get; init; }

    /// <summary>Gets or sets the event's explanation of its special rules.</summary>
    /// <value>Text (or a localization key) shown to players describing the derby's special rules, or <see langword="null"/> for an ordinary derby.</value>
    [JsonPropertyName("specialRulesText")]
    public string? SpecialRulesText { get; init; }

    /// <summary>Gets or sets the event-start notification text.</summary>
    /// <value>The notification shown when the derby starts, or <see langword="null"/> when the event sends none.</value>
    [JsonPropertyName("startNotification")]
    public string? StartNotification { get; init; }

    /// <summary>Gets or sets the price to bypass a task cooldown; zero retains the ordinary rule.</summary>
    /// <value>Diamonds charged to skip the wait between tasks (saved as <c>taskCooldownCost</c>).</value>
    [JsonPropertyName("taskCooldownCost")]
    public int TaskCooldownPrice { get; init; }

    /// <summary>Gets or sets the task cooldown in seconds; zero retains the ordinary rule.</summary>
    /// <value>Seconds a player waits between tasks (saved as <c>taskCooldownTime</c>).</value>
    [JsonPropertyName("taskCooldownTime")]
    public int TaskCooldownSeconds { get; init; }

    /// <summary>Gets or sets the event title.</summary>
    /// <value>The derby's display title (saved as <c>titleText</c>), or <see langword="null"/> to use the default title.</value>
    [JsonPropertyName("titleText")]
    public string? Title { get; init; }

    /// <summary>Gets or sets the native title style name.</summary>
    /// <value>The name of the client text style for the title, or <see langword="null"/> for the default style.</value>
    [JsonPropertyName("titleStyle")]
    public string? TitleStyle { get; init; }

    /// <summary>Gets or sets the shared event-board presentation.</summary>
    /// <value>The event-board appearance saved under <c>UI</c>, or <see langword="null"/> when the event defines none.</value>
    [JsonPropertyName("UI")]
    public EventUserInterfaceDefinition? UserInterface { get; init; }
}
