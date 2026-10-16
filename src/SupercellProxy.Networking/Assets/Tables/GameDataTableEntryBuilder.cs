using System.Collections.ObjectModel;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Collects the lines of one table entry while the parser walks the CSV, keeping each line's own cells and a running set of values
/// that later lines inherit.
/// </summary>
internal sealed class GameDataTableEntryBuilder(IReadOnlyList<string> headers, string name)
{
    private readonly Dictionary<string, LiteralValue> _currentValues = CreateInitialState(headers);
    private readonly List<IReadOnlyDictionary<string, LiteralValue>> _continuationRows = [];
    private readonly List<IReadOnlyDictionary<string, LiteralValue>> _snapshots = [];
    private ReadOnlyDictionary<string, LiteralValue>? _baseRow;

    /// <summary>
    /// Records the entry's named line; must come first and only once.
    /// </summary>
    /// <param name="values">Cells filled on the line, keyed by column header.</param>
    /// <exception cref="InvalidOperationException">The named line was already recorded.</exception>
    public void ApplyBaseRow(Dictionary<string, LiteralValue> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (_baseRow is not null)
            throw new InvalidOperationException(message: "Base row has already been applied.");

        ApplyValues(values);
        _baseRow = CreateReadOnlyCopy(values);
        _snapshots.Add(CreateReadOnlyCopy(_currentValues));
    }

    /// <summary>
    /// Records an unnamed line that follows the entry's named line and holds further array elements.
    /// </summary>
    /// <param name="values">Cells filled on the line, keyed by column header.</param>
    /// <exception cref="InvalidOperationException">No named line was recorded yet.</exception>
    public void ApplyContinuationRow(Dictionary<string, LiteralValue> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (_baseRow is null)
            throw new InvalidOperationException(message: "Cannot apply a continuation row before a base row.");

        ApplyValues(values);
        _continuationRows.Add(CreateReadOnlyCopy(values));
        _snapshots.Add(CreateReadOnlyCopy(_currentValues));
    }

    /// <summary>
    /// Freezes the recorded lines into an immutable entry.
    /// </summary>
    /// <exception cref="InvalidOperationException">No named line was recorded.</exception>
    public GameDataTableEntry Build()
    {
        return _baseRow is null
            ? throw new InvalidOperationException(message: "Cannot build an entry without a base row.")
            : new GameDataTableEntry(name, _baseRow, _continuationRows.AsReadOnly(), _snapshots.AsReadOnly());
    }

    private static Dictionary<string, LiteralValue> CreateInitialState(IReadOnlyList<string> headers)
    {
        Dictionary<string, LiteralValue> state = new(headers.Count, StringComparer.Ordinal);

        foreach (string header in headers)
            state[header] = default;

        return state;
    }

    private static ReadOnlyDictionary<string, LiteralValue> CreateReadOnlyCopy(Dictionary<string, LiteralValue> source)
    {
        return new ReadOnlyDictionary<string, LiteralValue>(new Dictionary<string, LiteralValue>(source, StringComparer.Ordinal));
    }

    private void ApplyValues(Dictionary<string, LiteralValue> values)
    {
        foreach (KeyValuePair<string, LiteralValue> pair in values)
            _currentValues[pair.Key] = pair.Value;
    }
}
