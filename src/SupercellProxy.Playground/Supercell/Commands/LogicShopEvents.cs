using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Shop-event collection carried by server command 355.
/// </summary>
public sealed record LogicShopEvents
{
    public const int MaxEventCount = 1024;

    public int Unknown0 { get; init; } = -1;
    public Memory<LogicShopEvent> Events { get; init; }

    internal static LogicShopEvents Decode(SupercellStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var eventCount = stream.ReadVarInt();

        if ((uint)eventCount > MaxEventCount)
            throw new InvalidDataException($"Invalid shop event count: {eventCount}.");

        var events = new LogicShopEvent[eventCount];

        for (var i = 0; i < events.Length; i++)
            events[i] = LogicShopEvent.Decode(stream);

        return new LogicShopEvents
        {
            Unknown0 = unknown0,
            Events = events
        };
    }

    internal void Encode(SupercellStream stream)
    {
        if (Events.Length > MaxEventCount)
            throw new InvalidDataException($"Invalid shop event count: {Events.Length}.");

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Events.Length);

        foreach (var shopEvent in Events.Span)
            shopEvent.Encode(stream);
    }
}
