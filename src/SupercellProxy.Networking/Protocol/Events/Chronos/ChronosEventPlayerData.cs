using System.Text.Json;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Retains the event identifier and its player-specific encoded document.</summary>
/// <param name="EventId">The id of the live event the document belongs to; in the saved array it precedes its document.</param>
/// <param name="Document">The player's state for that event as a JSON string, whose shape depends on the event type (for example <see cref="ChronosEventBonuses"/>).</param>
public sealed record ChronosEventPlayerData(int EventId, string Document)
{
    /// <summary>Reads the saved event state using its concrete contract.</summary>
    /// <typeparam name="TValue">The contract type the event's document is deserialized into, such as <see cref="ChronosEventBonuses"/>; it must be a reference type that System.Text.Json can create.</typeparam>
    /// <returns>A new <typeparamref name="TValue"/> filled from <see cref="Document"/>.</returns>
    /// <exception cref="InvalidDataException">Thrown when <see cref="Document"/> deserializes to <see langword="null"/> instead of an object.</exception>
    public TValue Decode<TValue>() where TValue : class
    {
        return JsonSerializer.Deserialize<TValue>(Document)
            ?? throw new InvalidDataException(message: "The saved event document is null.");
    }
}
