namespace SupercellProxy.Keys;

internal sealed class AppVersion
{
    private static readonly IComparer<string> ValueComparerInstance = Comparer<string>.Create(
        CompareValues
    );

    private AppVersion(string value, IReadOnlyList<string> sourceNames)
    {
        Value = value;
        SourceNames = sourceNames;
    }

    public string Value { get; }

    public IReadOnlyList<string> SourceNames { get; }

    public static IComparer<string> ValueComparer => ValueComparerInstance;

    public static IReadOnlyList<AppVersion> CreateMany(IEnumerable<string> sourceNames)
    {
        ArgumentNullException.ThrowIfNull(sourceNames);

        var aliasesByValue = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        var values = new List<string>();

        foreach (var sourceName in sourceNames)
        {
            var normalized = Normalize(sourceName);
            if (!aliasesByValue.TryGetValue(normalized, out var aliases))
            {
                aliases = [];
                aliasesByValue[normalized] = aliases;
                values.Add(normalized);
            }

            if (!aliases.Contains(sourceName, StringComparer.Ordinal))
                aliases.Add(sourceName);
        }

        return values
            .Select(value => new AppVersion(
                value,
                aliasesByValue[value]
                    .OrderBy(alias => string.Equals(alias, value, StringComparison.Ordinal) ? 0 : 1)
                    .ToArray()
            ))
            .ToArray();
    }

    public static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        var normalized = value.Trim();
        var prefixLength = 0;
        while (prefixLength < normalized.Length && normalized[prefixLength] is 'v' or 'V')
        {
            prefixLength++;
        }

        if (
            prefixLength > 0
            && prefixLength < normalized.Length
            && IsNumericVersion(normalized.AsSpan(prefixLength))
        )
        {
            return normalized[prefixLength..];
        }

        return normalized;
    }

    public override string ToString()
    {
        return Value;
    }

    private static int CompareValues(string? left, string? right)
    {
        if (ReferenceEquals(left, right))
            return 0;
        if (left is null)
            return -1;
        if (right is null)
            return 1;

        var leftNumeric = TryGetComponents(left, out var leftComponents);
        var rightNumeric = TryGetComponents(right, out var rightComponents);
        if (leftNumeric != rightNumeric)
            return leftNumeric ? 1 : -1;

        if (leftNumeric)
        {
            var componentCount = Math.Max(leftComponents.Length, rightComponents.Length);
            for (var index = 0; index < componentCount; index++)
            {
                var leftComponent = index < leftComponents.Length ? leftComponents[index] : "0";
                var rightComponent = index < rightComponents.Length ? rightComponents[index] : "0";
                var comparison = CompareNumericComponent(leftComponent, rightComponent);
                if (comparison is not 0)
                    return comparison;
            }
        }

        var ignoreCaseComparison = StringComparer.OrdinalIgnoreCase.Compare(left, right);
        return ignoreCaseComparison is not 0
            ? ignoreCaseComparison
            : StringComparer.Ordinal.Compare(left, right);
    }

    private static int CompareNumericComponent(string left, string right)
    {
        var normalizedLeft = left.AsSpan().TrimStart('0');
        var normalizedRight = right.AsSpan().TrimStart('0');
        if (normalizedLeft.Length != normalizedRight.Length)
            return normalizedLeft.Length.CompareTo(normalizedRight.Length);

        return normalizedLeft.SequenceCompareTo(normalizedRight);
    }

    private static bool IsNumericVersion(ReadOnlySpan<char> value)
    {
        var hasSeparator = false;
        var previousWasSeparator = true;
        foreach (var character in value)
        {
            if (character == '.')
            {
                if (previousWasSeparator)
                    return false;

                hasSeparator = true;
                previousWasSeparator = true;
                continue;
            }

            if (!char.IsAsciiDigit(character))
                return false;

            previousWasSeparator = false;
        }

        return hasSeparator && !previousWasSeparator;
    }

    private static bool TryGetComponents(string value, out string[] components)
    {
        if (!IsNumericVersion(value))
        {
            components = [];
            return false;
        }

        components = value.Split('.', StringSplitOptions.None);
        return true;
    }
}
