using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record PersonRouteState(IntPair[] EntryRoute, IntPair[] ExitRoute)
{
    private const string GameConfigFile = "data/game_config.csv";
    private const string StringValueField = "StringValue";

    public static PersonRouteState Resolve(DataTableResolver dataTableResolver)
    {
        return new PersonRouteState(
            ResolveRoute(dataTableResolver, "PeopleEntryRoute"),
            ResolveRoute(dataTableResolver, "PeopleExitRoute")
        );
    }

    private static IntPair[] ResolveRoute(DataTableResolver dataTableResolver, string routeName)
    {
        if (
            !dataTableResolver.TryResolveValueCount(
                GameConfigFile,
                routeName,
                StringValueField,
                out var pointCount
            )
            || pointCount < 1
        )
        {
            throw new InvalidDataException($"{GameConfigFile} has no {routeName} points.");
        }

        var points = new IntPair[pointCount];

        for (var i = 0; i < points.Length; i++)
        {
            if (
                !dataTableResolver.TryResolveString(
                    GameConfigFile,
                    routeName,
                    StringValueField,
                    i,
                    out var value
                )
            )
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"{GameConfigFile} has an invalid {routeName} point at index {i}."
                    )
                );

            points[i] = ParsePoint(value, routeName, i);
        }

        return points;
    }

    private static IntPair ParsePoint(string value, string routeName, int index)
    {
        var separator = value.IndexOf(';', StringComparison.Ordinal);

        if (separator < 1 || separator != value.LastIndexOf(';') || separator == value.Length - 1)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"{GameConfigFile} has a malformed {routeName} point at index {index}."
                )
            );

        return new IntPair(
            ParseCoordinate(value.AsSpan(0, separator), routeName, index),
            ParseCoordinate(value.AsSpan(separator + 1), routeName, index)
        );
    }

    private static int ParseCoordinate(ReadOnlySpan<char> value, string routeName, int index)
    {
        var negative = false;

        if (!value.IsEmpty && value[0] is '-' or '+')
        {
            negative = value[0] is '-';
            value = value[1..];
        }

        var decimalSeparator = value.IndexOf('.');
        var wholeText = decimalSeparator < 0 ? value : value[..decimalSeparator];
        var fractionText =
            decimalSeparator < 0 ? ReadOnlySpan<char>.Empty : value[(decimalSeparator + 1)..];

        if (
            wholeText.IsEmpty
            || fractionText.Length > 2
            || !int.TryParse(
                wholeText,
                System.Globalization.CultureInfo.InvariantCulture,
                out var whole
            )
            || (
                !fractionText.IsEmpty
                && !int.TryParse(
                    fractionText,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out _
                )
            )
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"{GameConfigFile} has a malformed {routeName} coordinate at index {index}."
                )
            );
        }

        var fraction = fractionText.IsEmpty ? 0 : int.Parse(fractionText);

        if (fractionText.Length is 1)
            fraction = checked(fraction * 10);

        var hundredths = checked(whole * 100 + fraction);

        if (negative)
            hundredths = checked(-hundredths);

        return checked(int.CreateChecked((long.CreateChecked(hundredths) * 0x200 / 100)));
    }
}
