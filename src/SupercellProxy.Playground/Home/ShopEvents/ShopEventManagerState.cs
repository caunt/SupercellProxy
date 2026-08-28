using SupercellProxy.Playground.Commands;

namespace SupercellProxy.Playground.Home;

internal sealed class ShopEventManagerState(AvatarManagerA snapshot)
{
    public AvatarManagerA Snapshot { get; private set; } = snapshot;

    public void Apply(ShopEventCollection? shopEvents)
    {
        if (shopEvents is null || shopEvents.Unknown0 is not 0 || shopEvents.Events.IsEmpty)
            return;

        var optional = Snapshot.Optional ?? new AvatarManagerAOptional(0, []);
        var entries = optional.Entries.ToList();
        var eventIds = entries.Select(static entry => entry.Unknown0).ToHashSet();

        for (var i = shopEvents.Events.Length - 1; i >= 0; i--)
        {
            var shopEvent = shopEvents.Events.Span[i];

            if (!eventIds.Add(shopEvent.EventId))
                continue;

            entries.Add(ToAvatarEntry(shopEvent));
        }

        var fixedValues = Snapshot.FixedValues.ToDictionary(
            static entry => entry.Key,
            static entry => entry.Value
        );

        foreach (var eventId in eventIds)
            fixedValues.TryAdd(eventId, 0);

        Snapshot = Snapshot with
        {
            Optional = optional with { Entries = entries.ToArray() },
            FixedValues = fixedValues
                .OrderBy(static entry => entry.Key)
                .Select(static entry => new KeyValuePair<int, int>(entry.Key, entry.Value))
                .ToArray(),
        };
    }

    private static AvatarManagerASpecial ToAvatarEntry(ShopEvent shopEvent)
    {
        return new AvatarManagerASpecial
        {
            UsesCompressedData = shopEvent.BinaryData is not null,
            Text = shopEvent.BinaryData is null ? shopEvent.TextData : null,
            CompressedData = shopEvent.BinaryData,
            Unknown0 = shopEvent.EventId,
            Unknown1 = shopEvent.Unknown0,
            UnknownString0 = shopEvent.UnknownString0,
            UnknownValues =
            [
                shopEvent.EventType,
                shopEvent.Unknown1,
                shopEvent.Unknown2,
                shopEvent.Unknown3,
                shopEvent.Unknown4,
                shopEvent.Unknown5,
                shopEvent.Unknown6,
                shopEvent.Unknown7,
                shopEvent.Unknown8,
                shopEvent.Unknown9,
                shopEvent.Unknown10,
            ],
            UnknownString1 = shopEvent.UnknownString1,
        };
    }
}
