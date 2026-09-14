using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents one retained time-limited gift-mailbox slot.</summary>
public sealed record GiftMailboxSlotSnapshot
{
    /// <summary>Gets the selected gift index.</summary>
    public int GiftIndex { get; init; }

    /// <summary>Gets the slot lifecycle state.</summary>
    public int GiftState { get; init; }

    /// <summary>Gets the slot timer.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? GiftTimer { get; init; }
}
