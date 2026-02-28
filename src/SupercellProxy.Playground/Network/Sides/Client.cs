using SupercellProxy.Playground.Exceptions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
using System.Net.Sockets;
using System.Text.Json.Nodes;

namespace SupercellProxy.Playground.Network.Sides;

public record ClientConfiguration(string UpstreamHost, int UpstreamPort, int MajorVersion, int MinorVersion, int PatchVersion, int ProtocolVersion, int KeyVersion);

public partial class Client(ClientConfiguration configuration) : IAsyncDisposable
{
    private TcpClient? tcpClient;
    private NetworkStream? _networkStream;
    private SupercellStream? _supercellStream;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var loginOkMessage = await LoginAsync(cancellationToken);
            Console.WriteLine(loginOkMessage);
        }
        catch (LoginException loginException)
        {
            Console.WriteLine($"Login failed: {loginException.Message}");
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Connection closed by remote host.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
        GC.SuppressFinalize(this);
    }

    private async Task<LoginOkMessage> LoginAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
            // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
            // 1.69.89  => 0c95746ec8ced89978f4b9fded2fdbc95b3daf18
            return await LoginAsync(fingerprintSha1: string.Empty, cancellationToken);
        }
        catch (LoginException loginException) when (loginException.LoginFailedMessage.ErrorCode is LoginFailedMessage.Type.OutdatedContent)
        {
            var node = JsonNode.Parse(loginException.LoginFailedMessage.ResourceFingerprintData);
            var fingerprint = node?["sha"]?.GetValue<string>();

            if (string.IsNullOrWhiteSpace(fingerprint))
                throw new InvalidOperationException($"Failed to parse fingerprint from login failed message:\n{loginException.LoginFailedMessage.ResourceFingerprintData}", loginException);

            return await LoginAsync(fingerprint, cancellationToken);
        }
    }

    private async Task<LoginOkMessage> LoginAsync(string fingerprintSha1, CancellationToken cancellationToken = default)
    {
        try
        {
            await WriteMessageAsync(new ClientHelloMessage
            {
                ProtocolVersion = configuration.ProtocolVersion,
                KeyVersion = configuration.KeyVersion,

                MajorVersion = configuration.MajorVersion,
                MinorVersion = configuration.MinorVersion,
                PatchVersion = configuration.PatchVersion,

                FingerprintSha1 = fingerprintSha1,

                DeviceType = 1,
                AppStore = 1,
                Unknown1 = -1
            }, cancellationToken);

            var message = await ReadMessageAsync(cancellationToken);
            LoginException.ThrowIfFailed(message);

            if (message is not ServerHelloMessage serverHello)
                throw new InvalidOperationException($"Expected {nameof(ServerHelloMessage)}, but received {message}.");

            var stream = await GetStreamAsync(cancellationToken);
            await stream.SetupEncryptionAsync(serverHello.SessionKey, cancellationToken);

            await WriteMessageAsync(new LoginMessage
            {
                AccountId = 0,
                PassToken = null,
                ResourceSha = fingerprintSha1,
                LoginVersion = 1119325,
                UdId = "",
                OpenUdId = "",
                MacAddress = "",
                DeviceModel = "iPad9,1",
                AdId = "",
                IsAdTracking = false,
                OsVersion = "18.2",
                Locale = "",
                Idfv = "",
                PreferredLanguage = "",
                ScidString = "",
                UnknownBool = true,
                ScIdToken = "",
                UnknownInt = -1,
                DataRef = -1,
                SystemString1 = "",
                SystemString2 = ""
            }, version: 5209, cancellationToken);

            message = await ReadMessageAsync(cancellationToken);
            LoginException.ThrowIfFailed(message);

            if (message is not LoginOkMessage loginOkMessage)
                throw new InvalidOperationException($"Expected {nameof(LoginOkMessage)}, but received {message}.");

            return loginOkMessage;
        }
        catch
        {
            await DisconnectAsync(cancellationToken);
            throw;
        }
    }

    private async Task WriteMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage
    {
        await WriteMessageAsync(message, version: 0 /* TODO: Always write message version here? */, cancellationToken);
    }

    private async Task WriteMessageAsync<T>(T message, ushort version, CancellationToken cancellationToken = default) where T : IMessage
    {
        var stream = await GetStreamAsync(cancellationToken);
        await stream.WriteMessageAsync(message.ToContainer(MessageRegistry.GetId<T>(), version: version), cancellationToken);
    }

    private async Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken)
    {
        var stream = await GetStreamAsync(cancellationToken);
        var container = await stream.ReadMessageAsync(cancellationToken);
        var message = MessageRegistry.Resolve(container);

        if (container.Payload.Position != container.Payload.Length)
            Console.WriteLine($"Warning: Not all payload data was consumed for message {message}. Remaining bytes: {container.Payload.Length - container.Payload.Position}");

        return message;
    }

    private async Task<T> ReadMessageAsync<T>(CancellationToken cancellationToken) where T : IMessage
    {
        var genericMessage = await ReadMessageAsync(cancellationToken);

        if (genericMessage is not T message)
            throw new InvalidOperationException($"Expected message {typeof(T)}, but received {genericMessage}.");

        return message;
    }

    private async Task<SupercellStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(configuration.UpstreamHost, configuration.UpstreamPort, cancellationToken);

            _networkStream = tcpClient.GetStream();
            _supercellStream = new SupercellStream(_networkStream);
        }

        return _supercellStream;
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is not null)
        {
            await _supercellStream.DisposeAsync();
            _supercellStream = null;
        }

        if (_networkStream is not null)
        {
            await _networkStream.DisposeAsync();
            _networkStream = null;
        }

        tcpClient?.Dispose();
        tcpClient = null;
    }
}