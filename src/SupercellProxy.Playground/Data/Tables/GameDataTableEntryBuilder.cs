using System.Collections.ObjectModel;

namespace SupercellProxy.Playground.Data.Tables;

internal sealed class GameDataTableEntryBuilder(IReadOnlyList<string> headers)
{
    private readonly Dictionary<string, object?> _currentValues = CreateInitialState(headers);
    private readonly List<IReadOnlyDictionary<string, object?>> _continuationRows = [];
    private readonly List<IReadOnlyDictionary<string, object?>> _snapshots = [];
    private ReadOnlyDictionary<string, object?>? _baseRow;

    public void ApplyBaseRow(Dictionary<string, object?> values)
    {
        if (_baseRow is not null)
            throw new InvalidOperationException("Base row has already been applied.");

        ApplyValues(values);
        _baseRow = CreateReadOnlyCopy(values);
        _snapshots.Add(CreateReadOnlyCopy(_currentValues));
    }

    public void ApplyContinuationRow(Dictionary<string, object?> values)
    {
        if (_baseRow is null)
            throw new InvalidOperationException(
                "Cannot apply a continuation row before a base row."
            );

        ApplyValues(values);
        _continuationRows.Add(CreateReadOnlyCopy(values));
        _snapshots.Add(CreateReadOnlyCopy(_currentValues));
    }

    public GameDataTableEntry Build()
    {
        if (_baseRow is null)
            throw new InvalidOperationException("Cannot build an entry without a base row.");

        var name =
            _baseRow.TryGetValue("Name", out var value) && value is string parsedName
                ? parsedName
                : string.Empty;
        return new GameDataTableEntry(
            name,
            _baseRow,
            _continuationRows.AsReadOnly(),
            _snapshots.AsReadOnly()
        );
    }

    private void ApplyValues(Dictionary<string, object?> values)
    {
        foreach (var pair in values)
            _currentValues[pair.Key] = pair.Value;
    }

    private static Dictionary<string, object?> CreateInitialState(IReadOnlyList<string> headers)
    {
        var state = new Dictionary<string, object?>(headers.Count, StringComparer.Ordinal);

        foreach (var header in headers)
            state[header] = null;

        return state;
    }

    private static ReadOnlyDictionary<string, object?> CreateReadOnlyCopy(
        Dictionary<string, object?> source
    )
    {
        return new ReadOnlyDictionary<string, object?>(
            new Dictionary<string, object?>(source, StringComparer.Ordinal)
        );
    }
}
