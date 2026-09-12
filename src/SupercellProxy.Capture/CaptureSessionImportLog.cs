using Microsoft.Extensions.Logging;

namespace SupercellProxy.Capture;

internal static class CaptureSessionImportLog
{
    private static readonly Action<ILogger, string, int, int, Exception?> CompletedMessage =
        LoggerMessage.Define<string, int, int>(
            LogLevel.Information,
            new EventId(id: 2, name: "SessionImportCompleted"),
            formatString: "Session ledger {LedgerPath} started with {InitialCount} existing sessions and imported {ImportedCount} sessions"
        );

    private static readonly Action<ILogger, string, Exception?> WarningMessage =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(id: 1, name: "SessionImportRejected"), formatString: "Session import rejected {Source}");

    internal static void Completed(ILogger logger, string ledgerPath, int initialCount, int importedCount)
    {
        CompletedMessage(logger, ledgerPath, initialCount, importedCount, arg5: null);
    }

    internal static void Warning(ILogger logger, string source, Exception exception)
    {
        WarningMessage(logger, source, exception);
    }
}
