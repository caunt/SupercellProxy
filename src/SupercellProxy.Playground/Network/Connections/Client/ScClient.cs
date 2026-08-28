using System.Globalization;
using System.Net.Sockets;
using System.Text.Json;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Configuration;
using SupercellProxy.Playground.Network.Connections.Client.Exceptions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Client;

/// <summary>
/// Represents <c language="csharp">ScClient</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="ScClient"/> instance.
/// </remarks>
internal sealed partial class ScClient : IAsyncDisposable
{
    private readonly ClientConfiguration? _configuration;
    private static readonly TimeSpan KeepAliveInterval = TimeSpan.FromSeconds(5);
    private readonly HttpClient _httpClient = new();
    private TcpClient? _tcpClient;
    private NetworkStream? _networkStream;
    private MessageStream? _supercellStream;

    /// Initializes an online client with the supplied connection configuration.
    public ScClient(ClientConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }

    /// Initializes a client over an existing message stream.
    internal ScClient(MessageStream messageStream)
    {
        ArgumentNullException.ThrowIfNull(messageStream);
        _supercellStream = messageStream;
    }

    /// Runs the client operation selected by command-line arguments.
    public static async Task RunAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    )
    {
        var operation = ParseOperation(arguments.ElementAtOrDefault(2));
        var (upstreamHost, upstreamPort) = await ConnectionAddress
            .ResolveAsync(arguments, cancellationToken)
            .ConfigureAwait(false);
        var client = new ScClient(
            new ClientConfiguration(
                upstreamHost,
                upstreamPort,
                ProtocolConfiguration.Current,
                arguments.ElementAtOrDefault(3)
            )
        );
        await using (client.ConfigureAwait(false))
        {
            switch (operation)
            {
                case ClientOperation.Run:
                    await client.RunAsync(cancellationToken).ConfigureAwait(false);
                    break;
                case ClientOperation.Harvest:
                    var result = await client
                        .HarvestFieldAsync(
                            ParseOptionalInt(arguments.ElementAtOrDefault(4)),
                            cancellationToken
                        )
                        .ConfigureAwait(false);
                    Console.WriteLine(JsonSerializer.Serialize(result));
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported client operation: {operation}."
                    );
            }
        }
    }

    private static ClientOperation ParseOperation(string? value)
    {
        return
            Enum.TryParse<ClientOperation>(value, ignoreCase: true, out var operation)
            && Enum.IsDefined(operation)
            ? operation
            : ClientOperation.Run;
    }

    private static int? ParseOptionalInt(string? value)
    {
        if (value is null)
            return null;

        return int.TryParse(value, CultureInfo.InvariantCulture, out var result)
            ? result
            : throw new ArgumentException($"Invalid integer value: {value}.", nameof(value));
    }

    private ClientConfiguration Configuration =>
        _configuration
        ?? throw new InvalidOperationException(
            "This client was created without an online connection configuration."
        );

    /// <summary>
    /// Executes the <c language="csharp">RunAsync</c> operation.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var loginOkResult = await LoginAsync(cancellationToken).ConfigureAwait(false);

            Console.WriteLine(ApplicationText.ClientLoggedIn);

            if (loginOkResult.Resources.Length > 0)
                (
                    await GetStreamAsync(cancellationToken).ConfigureAwait(false)
                ).CommandDataResolver = new DataTableResolver(loginOkResult.Resources);

            var keepAliveTask = Task.Run(
                async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(KeepAliveInterval, TimeProvider.System, cancellationToken)
                            .ConfigureAwait(false);

                        var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
                        await stream
                            .WriteMessageAsync(new KeepAliveMessage(), cancellationToken)
                            .ConfigureAwait(false);
                    }
                },
                cancellationToken
            );

            HandleGoods(loginOkResult.Resources);

            while (!keepAliveTask.IsCompleted)
                await HandleIncomingMessageAsync(cancellationToken).ConfigureAwait(false);

            await keepAliveTask.ConfigureAwait(false);
        }
        catch (LoginException loginException)
        {
            Console.WriteLine($"Login failed: {loginException.Message}");
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine(ApplicationText.ClientConnectionClosed);
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">DisposeAsync</c> operation.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync().ConfigureAwait(false);
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets <c language="csharp">ReadyFieldsAsync</c>.
    /// </summary>
    public async Task<HarvestField[]> GetReadyFieldsAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await EnsureHomeReadyAsync(cancellationToken).ConfigureAwait(false);
            return FieldPlots.GetReady();
        }
        finally
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">ValidateSynchronizationAsync</c> operation.
    /// </summary>
    public async Task ValidateSynchronizationAsync(
        int subTick,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await EnsureHomeReadyAsync(cancellationToken).ConfigureAwait(false);
            var state = HomeTurns.State;

            if (subTick < state.Tick.SubTick)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(subTick),
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"The first synchronization boundary is sub-tick {state.Tick.SubTick}."
                    )
                );
            }

            var turn = HomeTurns.CreateSynchronizationTurn(subTick);
            await SendClientTurnAsync(turn, cancellationToken).ConfigureAwait(false);
            HomeTurns.ConfirmTurnSent(turn);
        }
        finally
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// <para>Harvests the selected field, or the first genuinely ready field when no global ID is supplied.</para>
    /// </summary>
    public async Task<HarvestResult> HarvestFieldAsync(
        int? fieldGlobalId = null,
        CancellationToken cancellationToken = default
    )
    {
        Console.WriteLine(ApplicationText.ClientLoadingHarvestState);
        FieldHarvestVerification firstHarvest;

        try
        {
            await EnsureHomeReadyAsync(cancellationToken).ConfigureAwait(false);
            firstHarvest = await FieldPlots
                .HarvestAsync(fieldGlobalId, SendClientTurnAsync, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }

        Console.WriteLine(ApplicationText.ClientRestartingForHarvestVerification);
        var verificationClient = new ScClient(Configuration);
        await using (verificationClient.ConfigureAwait(false))
        {
            await verificationClient.EnsureHomeReadyAsync(cancellationToken).ConfigureAwait(false);
            return FieldPlots.Verify(firstHarvest, verificationClient.FieldPlots);
        }
    }

    private static void ValidateTurnRoundTrip(
        EndClientTurnMessage turn,
        ICommandDataResolver? dataResolver
    )
    {
        var id = MessageRegistry.GetId<EndClientTurnMessage>();
        var version = MessageRegistry.GetVersion<EndClientTurnMessage>();
        var container = turn.ToContainer(id, version);
        var payload = container.Payload.ToArray();
        container.Payload.Position = 0;
        var decoded = EndClientTurnMessage.Create(
            container,
            CommandEnvironment.Production,
            dataResolver
        );
        var reencoded = decoded.ToContainer(id, version).Payload.ToArray();

        if (!payload.AsSpan().SequenceEqual(reencoded))
            throw new InvalidDataException(
                $"{nameof(EndClientTurnMessage)} did not round-trip byte-exactly before sending."
            );
    }
}
