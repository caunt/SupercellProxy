using System.Collections.ObjectModel;

namespace SupercellProxy.Playground.Data.Tables;

internal sealed class GameDataTableEntryBuilder(IReadOnlyList<string> headers)
{
    private readonly Dictionary<string, object?> currentValues = CreateInitialState(headers);
    private readonly List<IReadOnlyDictionary<string, object?>> continuationRows = [];
    private readonly List<IReadOnlyDictionary<string, object?>> snapshots = [];
    private IReadOnlyDictionary<string, object?>? baseRow;

    public void ApplyBaseRow(Dictionary<string, object?> values)
    {
        if (baseRow is not null)
            throw new InvalidOperationException("Base row has already been applied.");

        ApplyValues(values);
        baseRow = CreateReadOnlyCopy(values);
        snapshots.Add(CreateReadOnlyCopy(currentValues));
    }

    public void ApplyContinuationRow(Dictionary<string, object?> values)
    {
        if (baseRow is null)
            throw new InvalidOperationException(
                "Cannot apply a continuation row before a base row."
            );

        ApplyValues(values);
        continuationRows.Add(CreateReadOnlyCopy(values));
        snapshots.Add(CreateReadOnlyCopy(currentValues));
    }

    public GameDataTableEntry Build()
    {
        if (baseRow is null)
            throw new InvalidOperationException("Cannot build an entry without a base row.");

        var name =
            baseRow.TryGetValue("Name", out var value) && value is string parsedName
                ? parsedName
                : string.Empty;
        return new GameDataTableEntry(
            name,
            baseRow,
            continuationRows.AsReadOnly(),
            snapshots.AsReadOnly()
        );
    }

    private void ApplyValues(Dictionary<string, object?> values)
    {
        foreach (var pair in values)
            currentValues[pair.Key] = pair.Value;
    }

    private static Dictionary<string, object?> CreateInitialState(IReadOnlyList<string> headers)
    {
        var state = new Dictionary<string, object?>(headers.Count, StringComparer.Ordinal);

        foreach (var header in headers)
            state[header] = null;

        return state;
    }

    private static IReadOnlyDictionary<string, object?> CreateReadOnlyCopy(
        Dictionary<string, object?> source
    )
    {
        return new ReadOnlyDictionary<string, object?>(
            new Dictionary<string, object?>(source, StringComparer.Ordinal)
        );
    }
}
