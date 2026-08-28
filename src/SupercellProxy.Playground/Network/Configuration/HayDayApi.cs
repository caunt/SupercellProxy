using System.Text.RegularExpressions;

namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Represents <c language="csharp">HayDayApi</c>.
/// </summary>
internal static partial class HayDayApi
{
    private static readonly HttpClient HttpClient = new();
    private static readonly Uri KeysUri = new(
        "https://raw.githubusercontent.com/caunt/SupercellProxy/refs/heads/main/KEYS.md"
    );

    /// <summary>
    /// Gets <c language="csharp">ServerPublicKeyAsync</c>.
    /// </summary>
    public static async ValueTask<byte[]> GetServerPublicKeyAsync(
        CancellationToken cancellationToken = default
    )
    {
        var content = await HttpClient
            .GetStringAsync(KeysUri, cancellationToken)
            .ConfigureAwait(false);
        var hayDayMatch = HayDayPublicKeyRegex.Match(content);

        if (!hayDayMatch.Success)
            throw new InvalidOperationException("Hay Day key not found.");

        return Convert.FromHexString(hayDayMatch.Groups["key"].Value);
    }

    [GeneratedRegex(
        @"(?ms)^##[^\r\n]*Hay Day[^\r\n]*\r?\n.*?`(?<key>[0-9A-Fa-f]{64})`",
        RegexOptions.None | RegexOptions.ExplicitCapture,
        1_000
    )]
    private static partial Regex HayDayPublicKeyRegex { get; }
}
