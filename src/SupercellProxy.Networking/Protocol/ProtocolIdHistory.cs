namespace SupercellProxy.Networking.Protocol;

internal sealed class ProtocolIdHistory(int? baselineId, ProtocolIdChange[] changes)
{
    private readonly ProtocolIdChange[] _changes = [.. changes.OrderBy(static change => change.Since)];

    internal IEnumerable<int> Ids
    {
        get
        {
            if (baselineId is { } initialId)
                yield return initialId;

            foreach (ProtocolIdChange change in _changes)
            {
                if (change.Id is { } id)
                    yield return id;
            }
        }
    }

    internal int? GetId(Version? gameVersion)
    {
        if (gameVersion is null)
            return baselineId;

        int lower = 0;
        int upper = _changes.Length;

        while (lower < upper)
        {
            int middle = lower + ((upper - lower) / 2);

            if (_changes[middle].Since <= gameVersion)
                lower = middle + 1;
            else
                upper = middle;
        }

        return lower == 0 ? baselineId : _changes[lower - 1].Id;
    }
}
