namespace SupercellProxy.Networking.Cryptography;

internal sealed class HayDayServerPublicKeySource(HttpClient webClient) : IServerPublicKeySource
{
    private static readonly Uri KeysAddress = new(uriString: "https://raw.githubusercontent.com/caunt/SupercellProxy/refs/heads/main/KEYS.md");

    public async ValueTask<byte[]> GetServerPublicKeyAsync(CancellationToken cancellationToken = default)
    {
        string content = await webClient
            .GetStringAsync(KeysAddress, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        bool selected = false;

        foreach (string line in content.Split(separator: '\n'))
        {
            if (line.StartsWith(value: "##", StringComparison.Ordinal))
                selected = line.Contains(value: "Hay Day", StringComparison.Ordinal);

            if (!selected)
                continue;

            foreach (string token in line.Split(separator: '`'))
            {
                if (token.Length == 64 && token.All(Uri.IsHexDigit))
                    return Convert.FromHexString(token);
            }
        }

        throw new InvalidDataException(message: "The Hay Day server public key was not found.");
    }
}
