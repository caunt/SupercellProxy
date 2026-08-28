using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Shop-event collection carried by server command 355.</para>
/// </summary>
internal sealed record ShopEventCollection
{
    /// <summary>
    /// Defines the <c language="csharp">MaxEventCount</c> value.
    /// </summary>
    public const int MaxEventCount = 1024;

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; } = -1;

    /// <summary>
    /// Gets or sets the <c language="csharp">Events</c> value.
    /// </summary>
    public Memory<ShopEvent> Events { get; init; }

    internal static ShopEventCollection Decode(MessageStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var eventCount = stream.ReadVarInt();

        if (uint.CreateTruncating(eventCount) > MaxEventCount)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid shop event count: {eventCount}."
                )
            );

        var events = new ShopEvent[eventCount];

        for (var i = 0; i < events.Length; i++)
            events[i] = ShopEvent.Decode(stream);

        return new ShopEventCollection { Unknown0 = unknown0, Events = events };
    }

    internal void Encode(MessageStream stream)
    {
        if (Events.Length > MaxEventCount)
            throw new InvalidDataException($"Invalid shop event count: {Events.Length}.");

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Events.Length);

        foreach (var shopEvent in Events.Span)
            shopEvent.Encode(stream);
    }
}
