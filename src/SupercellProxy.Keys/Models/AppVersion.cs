namespace SupercellProxy.Keys.Models;

internal sealed class AppVersion
{
    private AppVersion(string value, IReadOnlyList<string> sourceNames)
    {
        Value = value;
        SourceNames = sourceNames;
    }

    public static IComparer<string> ValueComparer { get; } = Comparer<string>.Create(CompareValues);

    public IReadOnlyList<string> SourceNames { get; }

    public string Value { get; }

    public static IReadOnlyList<AppVersion> CreateMany(IEnumerable<string> sourceNames)
    {
        ArgumentNullException.ThrowIfNull(sourceNames);

        Dictionary<string, List<string>> aliasesByValue = new(StringComparer.Ordinal);
        List<string> values = [];

        foreach (string sourceName in sourceNames)
        {
            string normalized = Normalize(sourceName);

            if (!aliasesByValue.TryGetValue(normalized, out List<string>? aliases))
            {
                aliases = [];
                aliasesByValue[normalized] = aliases;
                values.Add(normalized);
            }

            if (!aliases.Contains(sourceName, StringComparer.Ordinal))
                aliases.Add(sourceName);
        }

        return [.. values
            .Select(
                value => new AppVersion(value, [..aliasesByValue[value].OrderBy(alias => string.Equals(alias, value, StringComparison.Ordinal) ? 0 : 1)])
            )];
    }

    public static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        string normalized = value.Trim();
        int prefixLength = 0;

        while (prefixLength < normalized.Length && normalized[prefixLength] is 'v' or 'V')
            prefixLength++;

        return prefixLength > 0 && prefixLength < normalized.Length && IsNumericVersion(normalized.AsSpan(prefixLength))
            ? normalized[prefixLength..]
            : normalized;
    }

    public override string ToString()
    {
        return Value;
    }

    private static int CompareNumericComponent(string left, string right)
    {
        ReadOnlySpan<char> normalizedLeft = left.AsSpan().TrimStart(trimChar: '0');
        ReadOnlySpan<char> normalizedRight = right.AsSpan().TrimStart(trimChar: '0');

        return normalizedLeft.Length != normalizedRight.Length
            ? normalizedLeft.Length.CompareTo(normalizedRight.Length)
            : normalizedLeft.SequenceCompareTo(normalizedRight);
    }

    private static int CompareValues(string? left, string? right)
    {
        if (ReferenceEquals(left, right))
            return 0;

        if (left is null)
            return -1;

        if (right is null)
            return 1;

        bool leftNumeric = TryGetComponents(left, out string[]? leftComponents);
        bool rightNumeric = TryGetComponents(right, out string[]? rightComponents);

        if (leftNumeric != rightNumeric)
            return leftNumeric ? 1 : -1;

        if (leftNumeric)
        {
            int componentCount = Math.Max(leftComponents.Length, rightComponents.Length);

            for (int index = 0; index < componentCount; index++)
            {
                string leftComponent = index < leftComponents.Length ? leftComponents[index] : "0";
                string rightComponent = index < rightComponents.Length ? rightComponents[index] : "0";
                int comparison = CompareNumericComponent(leftComponent, rightComponent);

                if (comparison is not 0)
                    return comparison;
            }
        }

        int ignoreCaseComparison = StringComparer.OrdinalIgnoreCase.Compare(left, right);

        return ignoreCaseComparison is not 0
            ? ignoreCaseComparison
            : StringComparer.Ordinal.Compare(left, right);
    }

    private static bool IsNumericVersion(ReadOnlySpan<char> value)
    {
        bool hasSeparator = false;
        bool previousWasSeparator = true;

        foreach (char character in value)
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

        components = value.Split(separator: '.', StringSplitOptions.None);

        return true;
    }
}
