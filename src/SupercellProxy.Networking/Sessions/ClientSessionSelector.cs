using System.Globalization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Resolves a ledger session by farm-name fragment or numbered console selection.</summary>
public sealed class ClientSessionSelector
{
    private readonly TextReader _input;
    private readonly TextWriter _output;

    /// <summary>Initializes a selector with its interactive input and output streams.</summary>
    public ClientSessionSelector(TextReader input, TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(output);
        _input = input;
        _output = output;
    }

    /// <summary>Selects a session according to the host's account-option and default-selection policy.</summary>
    /// <returns>The selected session, or null when selection is optional and was not requested.</returns>
    public async Task<ClientSession?> SelectAsync(
        ClientSessionLedger ledger,
        bool accountOptionSpecified,
        string? farmNameFragment,
        bool selectionRequired,
        bool inputRedirected,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(ledger);

        if (!accountOptionSpecified && !selectionRequired)
            return null;

        ClientSession[] sessions = await ledger.GetSessionsAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (sessions.Length is 0)
            throw new InvalidOperationException($"No sessions are saved in {ledger.FilePath}.");

        string? fragment = string.IsNullOrWhiteSpace(farmNameFragment) ? null : farmNameFragment.Trim();

        if (fragment is not null)
        {
            ClientSession[] matches = [.. sessions.Where(session => session.FarmName.Contains(fragment, StringComparison.OrdinalIgnoreCase))];

            if (matches.Length is 1)
                return matches[0];

            if (matches.Length > 1)
                return await PromptAsync(matches, inputRedirected, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            await _output.WriteLineAsync($"No farm name contains \"{fragment}\"; showing every saved session.")
                .ConfigureAwait(continueOnCapturedContext: false);

            return await PromptAsync(sessions, inputRedirected, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        return sessions.Length is 1
            ? sessions[0]
            : await PromptAsync(sessions, inputRedirected, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task<ClientSession> PromptAsync(ClientSession[] sessions, bool inputRedirected, CancellationToken cancellationToken)
    {
        if (inputRedirected)
            throw new InvalidOperationException(message: "Interactive session selection is unavailable; pass --account FARM_NAME.");

        await _output.WriteLineAsync(value: "Select a session:").ConfigureAwait(continueOnCapturedContext: false);

        for (int index = 0; index < sessions.Length; index++)
        {
            ClientSession session = sessions[index];
            await _output
                .WriteLineAsync(
                    string.Create(CultureInfo.InvariantCulture, $"{index + 1}. {session.FarmName} ({session.AccountIdentifier.ToFormattedString()})")
                )
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _output.WriteAsync(string.Create(CultureInfo.InvariantCulture, $"Choose 1-{sessions.Length}: "))
                .ConfigureAwait(continueOnCapturedContext: false);
            await _output.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            string? value = await _input.ReadLineAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false) ?? throw new InvalidOperationException(message: "Interactive session selection ended; pass --account FARM_NAME.");

            bool validSelection = int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out int selected)
                && selected >= 1
                && selected <= sessions.Length;

            if (validSelection)
                return sessions[selected - 1];

            await _output.WriteLineAsync(value: "Enter one of the displayed numbers.").ConfigureAwait(continueOnCapturedContext: false);
        }

        throw new OperationCanceledException(cancellationToken);
    }
}
