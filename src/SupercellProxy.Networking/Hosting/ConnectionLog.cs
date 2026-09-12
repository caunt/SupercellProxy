using Microsoft.Extensions.Logging;

namespace SupercellProxy.Networking.Hosting;

internal static class ConnectionLog
{
    private static readonly Action<ILogger, string, Exception?> Information =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(id: 1, name: "Connection"), formatString: "{Message}");

    private static readonly Action<ILogger, string, Exception?> DebugMessage =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(id: 2, name: "ConnectionStopped"), formatString: "{Message}");

    internal static void Debug(ILogger logger, string message)
    {
        DebugMessage(logger, message, arg3: null);
    }

    internal static void Write(ILogger logger, string message)
    {
        Information(logger, message, arg3: null);
    }
}
