using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents one retained time-limited gift-mailbox slot.</summary>
public sealed record GiftMailboxSlotSnapshot
{
    /// <summary>Gets or sets the selected gift index.</summary>
    /// <value>A zero-based index into the <c>Gifts</c> list of the time-limited catalogue slot (<c>slot4</c>).</value>
    public int GiftIndex { get; init; }

    /// <summary>Gets or sets the slot lifecycle state.</summary>
    /// <value>0 while the gift is on offer and 1 while the slot cools down after the offer expired or was bought; other states are not identified.</value>
    public int GiftState { get; init; }

    /// <summary>Gets or sets the slot timer.</summary>
    /// <value>The countdown of the current state, the offer or the cooldown after it; <see langword="null"/> when not saved.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? GiftTimer { get; init; }
}
