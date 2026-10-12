using System.Text.Json;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Retains the event identifier and its player-specific encoded document.</summary>
public sealed record ChronosEventPlayerData(int EventId, string Document)
{
    /// <summary>Reads the saved event state using its concrete contract.</summary>
    public TValue Decode<TValue>() where TValue : class
    {
        return JsonSerializer.Deserialize<TValue>(Document)
            ?? throw new InvalidDataException(message: "The saved event document is null.");
    }
}
