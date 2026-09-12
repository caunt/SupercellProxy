using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Shop-event collection carried by server command 355.</para>
/// </summary>
public sealed record ShopEventCollection
{
    /// <summary>
    /// Defines the <c language="csharp">MaxEventCount</c> value.
    /// </summary>
    public const int MaximumEventCount = 1024;

    /// <summary>
    /// Gets or sets the <c language="csharp">Events</c> value.
    /// </summary>
    public Memory<ShopEvent> Events { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; } = -1;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ShopEventCollection Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int unknown0 = stream.ReadVariableInt();
        int eventCount = stream.ReadVariableInt();

        if (uint.CreateTruncating(eventCount) > MaximumEventCount)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid shop event count: {eventCount}."));

        ShopEvent[] events = new ShopEvent[eventCount];

        for (int index = 0; index < events.Length; index++)
            events[index] = ShopEvent.Decode(stream);

        return new ShopEventCollection { Unknown0 = unknown0, Events = events };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (Events.Length > MaximumEventCount)
            throw new InvalidDataException($"Invalid shop event count: {Events.Length}.");

        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Events.Length);

        foreach (ShopEvent shopEvent in Events.Span)
            shopEvent.Encode(stream);
    }
}
