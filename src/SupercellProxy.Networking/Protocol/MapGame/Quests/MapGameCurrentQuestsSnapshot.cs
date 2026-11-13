using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Today's Valley daily quests.</summary>
public sealed record MapGameCurrentQuestsSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets each of today's daily quests with its progress.</summary>
    /// <value>One entry per quest in saved order; empty by default.</value>
    [JsonPropertyName("Quests")]
    public ImmutableArray<MapGameDailyQuestSnapshot> Quests { get; init; } = [];
}
