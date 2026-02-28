using SupercellProxy.Playground.Exceptions;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using System.Text.Json.Nodes;

namespace SupercellProxy.Playground.Network.Sides;

public record ClientConfiguration(string UpstreamHost, int UpstreamPort, ProtocolConfiguration Protocol);

public partial class Client(ClientConfiguration configuration) : IAsyncDisposable
{
    private static readonly TimeSpan _keepAliveInterval = TimeSpan.FromSeconds(5);

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var loginOkMessage = await LoginAsync(cancellationToken);

            Console.WriteLine(loginOkMessage);

            var keepAliveTask = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(_keepAliveInterval);

                    var stream = await GetStreamAsync(cancellationToken);
                    await stream.WriteMessageAsync(new KeepAliveMessage(), cancellationToken);
                }
            }, cancellationToken);

            while (!keepAliveTask.IsCompleted)
                await HandleIncomingMessageAsync(cancellationToken);

            await keepAliveTask;
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
            var stream = await GetStreamAsync(cancellationToken);

            await stream.WriteMessageAsync(new ClientHelloMessage
            {
                ProtocolVersion = configuration.Protocol.ProtocolVersion,
                KeyVersion = configuration.Protocol.KeyVersion,

                MajorVersion = configuration.Protocol.MajorVersion,
                MinorVersion = configuration.Protocol.MinorVersion,
                PatchVersion = configuration.Protocol.PatchVersion,

                FingerprintSha1 = fingerprintSha1,

                DeviceType = 1,
                AppStore = 1,
                Unknown1 = -1
            }, cancellationToken);

            var message = await stream.ReadMessageAsync(cancellationToken);
            LoginException.ThrowIfFailed(message);

            if (message is not ServerHelloMessage serverHello)
                throw new InvalidOperationException($"Expected {nameof(ServerHelloMessage)}, but received {message}.");

            await stream.SetupEncryptionAsync(serverHello.SessionKey, cancellationToken);

            await stream.WriteMessageAsync(new LoginMessage
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
            }, cancellationToken);

            message = await stream.ReadMessageAsync(cancellationToken);
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
}