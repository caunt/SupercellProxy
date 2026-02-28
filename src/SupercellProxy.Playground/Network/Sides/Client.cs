using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
using System.Net.Sockets;

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
            await LoginAsync(cancellationToken);
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Connection closed by remote host.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_supercellStream is not null)
            await _supercellStream.DisposeAsync();

        if (_networkStream is not null)
            await _networkStream.DisposeAsync();

        tcpClient?.Dispose();

        GC.SuppressFinalize(this);
    }

    private async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        await WriteMessageAsync(new ClientHelloMessage
        {
            ProtocolVersion = configuration.ProtocolVersion,
            KeyVersion = configuration.KeyVersion,

            MajorVersion = configuration.MajorVersion,
            MinorVersion = configuration.MinorVersion,
            PatchVersion = configuration.PatchVersion,

            // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
            // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
            // 1.69.89  => 0c95746ec8ced89978f4b9fded2fdbc95b3daf18
            FingerprintSha1 = "0c95746ec8ced89978f4b9fded2fdbc95b3daf18",

            DeviceType = 1,
            AppStore = 1,
            Unknown1 = -1
        }, cancellationToken);

        await SetupEncryptionAsync(cancellationToken);

        await WriteMessageAsync(new LoginMessage
        {
            AccountId = 0,
            PassToken = null,
            ResourceSha = "0c95746ec8ced89978f4b9fded2fdbc95b3daf18",
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

        var container = await ReadMessageAsync(cancellationToken);

        if (container.Id == LoginFailedMessage.Id)
            Console.WriteLine($"Login failed: {LoginFailedMessage.Create(container)}");
        else if (container.Id == LoginOkMessage.Id)
            Console.WriteLine($"Login successful: {LoginOkMessage.Create(container)}");
    }

    public async Task SetupEncryptionAsync(CancellationToken cancellationToken = default)
    {
        var serverHello = await ReadMessageAsync<ServerHelloMessage>(cancellationToken);

        var stream = await GetStreamAsync(cancellationToken);
        await stream.SetupEncryptionAsync(serverHello.SessionKey, cancellationToken);
    }

    private async Task WriteMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage
    {
        await WriteMessageAsync(message, version: 0 /* TODO: Always write message version here? */, cancellationToken);
    }

    private async Task WriteMessageAsync<T>(T message, ushort version, CancellationToken cancellationToken = default) where T : IMessage
    {
        var stream = await GetStreamAsync(cancellationToken);
        await stream.WriteMessageAsync(message.ToContainer(T.Id, version: version), cancellationToken);
    }

    private async Task<MessageContainer> ReadMessageAsync(CancellationToken cancellationToken)
    {
        var stream = await GetStreamAsync(cancellationToken);
        var container = await stream.ReadMessageAsync(cancellationToken);

        return container;
    }

    private async Task<T> ReadMessageAsync<T>(CancellationToken cancellationToken) where T : IMessage
    {
        var container = await ReadMessageAsync(cancellationToken);

        if (container.Id != T.Id)
            throw new InvalidOperationException($"Expected message ID {T.Id}, but received {container.Id}.");

        if (T.Create(container) is not T message)
            throw new InvalidOperationException($"Failed to create message of type {typeof(T)} from container.");

        if (container.Payload.Position != container.Payload.Length)
            Console.WriteLine($"Warning: Not all payload data was consumed for message {typeof(T)}. Remaining bytes: {container.Payload.Length - container.Payload.Position}");

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
}