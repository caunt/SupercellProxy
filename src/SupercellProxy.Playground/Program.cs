using SupercellProxy.Playground;
using SupercellProxy.Playground.Network.Connections.Client;
using SupercellProxy.Playground.Network.Connections.Proxy;
using SupercellProxy.Playground.Network.Connections.Replay;
using SupercellProxy.Playground.Network.Connections.Server;

var cancellationToken = CancellationToken.None;
var (runMode, connectionArguments) = ResolveRunMode(args);

switch (runMode)
{
    case RunMode.Client:
        await ScClient.RunAsync(connectionArguments, cancellationToken).ConfigureAwait(false);
        break;
    case RunMode.Proxy:
        await ScProxy.RunAsync(connectionArguments, cancellationToken).ConfigureAwait(false);
        break;
    case RunMode.Replay:
        await ScReplay.RunAsync(connectionArguments, cancellationToken).ConfigureAwait(false);
        break;
    case RunMode.Server:
        await ScServer
            .Create(connectionArguments)
            .RunAsync(cancellationToken)
            .ConfigureAwait(false);
        break;
    default:
        throw new InvalidOperationException($"Unsupported run mode: {runMode}.");
}

static (RunMode Mode, string[] Arguments) ResolveRunMode(string[] arguments)
{
    if (
        !Enum.TryParse<RunMode>(arguments.ElementAtOrDefault(0), ignoreCase: true, out var mode)
        || !Enum.IsDefined(mode)
    )
        return (RunMode.Proxy, arguments);

    return (mode, arguments[1..]);
}
