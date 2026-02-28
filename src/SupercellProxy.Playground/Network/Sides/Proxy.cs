using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;
using System.Net;
using System.Net.Sockets;

namespace SupercellProxy.Playground.Network.Sides;

// There are multiple ways to implement a proxy that can decrypt packets.
// 1) Get the 'client private key' from the app and give it to the proxy (not sure but maybe 'snonce' needed as well).
// 2) Replace the 'server public key' in the app with 'proxy public key', re-encrypt packets on the proxy.
// CoCSharp.Proxy uses second approach - https://github.com/FICTURE7/CoCSharp/blob/d8602264fd185a9236197502eb40aa57019bf4be/src/CoCSharp.Proxy/MessageProcessorNaClProxy.cs#L99
// They introduced a "standard" key pair for modded servers at Crypto8.StandardKeyPair:
// standard(proxy) public key: 72F1A4A4C48E44DA0C42310F800E96624E6DC6A641A9D41C3B5039D8DFADC27E
// standard(proxy) public key encoded: 5E2E00002929000047620000DA440000841800003CC400007400000029660000CDA90000A9B10000D4A000001CD40000A076000060E700006EFD0000EC27000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
// If the goal is just to log unencrypted packets, maybe patch the app to log them directly.

// private static readonly byte[] _standardPrivateKey = new byte[]
// {
//         0x18, 0x91, 0xD4, 0x01, 0xFA, 0xDB, 0x51, 0xD2, 0x5D, 0x3A, 0x91, 0x74,
//         0xD4, 0x72, 0xA9, 0xF6, 0x91, 0xA4, 0x5B, 0x97, 0x42, 0x85, 0xD4, 0x77,
//         0x29, 0xC4, 0x5C, 0x65, 0x38, 0x07, 0x0D, 0x85
// };
// 
// private static readonly byte[] _standardPublicKey = new byte[] // == PublicKeyBox.GenerateKeyPair(_standardPrivateKey);
// {
//         0x72, 0xF1, 0xA4, 0xA4, 0xC4, 0x8E, 0x44, 0xDA, 0x0C, 0x42, 0x31, 0x0F,
//         0x80, 0x0E, 0x96, 0x62, 0x4E, 0x6D, 0xC6, 0xA6, 0x41, 0xA9, 0xD4, 0x1C,
//         0x3B, 0x50, 0x39, 0xD8, 0xDF, 0xAD, 0xC2, 0x7E
// };

public record ProxyConfiguration(string UpstreamHost, int UpstreamPort, string ListenAddress, int ListenPort, ProtocolConfiguration Protocol);

public class Proxy(ProxyConfiguration configuration)
{
    // https://github.com/ReversedCell/ScDocumentation/wiki/Encryption-Setup
    // https://github.com/ReversedCell/ScDocumentation/wiki/Protocol

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var listener = new TcpListener(IPAddress.Parse(configuration.ListenAddress), configuration.ListenPort);
        listener.Start();
        Console.WriteLine($"[{DateTime.Now:T}] Listening on {configuration.ListenAddress}:{configuration.ListenPort}, upstream {configuration.UpstreamHost}:{configuration.UpstreamPort}");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(cancellationToken);
            _ = HandleClientAsync(client, configuration.UpstreamHost, configuration.UpstreamPort, cancellationToken);
        }
    }

    private static async ValueTask PacketSentAsync(MessageContainer container, Direction direction, CancellationToken cancellationToken = default)
    {
        switch (container.Id)
        {
            case 10100:
                Console.WriteLine($"[{DateTime.Now:T}] {ClientHelloMessage.Create(container)}");
                return;
            case 20100:
                Console.WriteLine($"[{DateTime.Now:T}] {ServerHelloMessage.Create(container)}");
                return;
            case 10101:
                var serverPublicKey = await HayDayApi.GetServerPublicKeyAsync(cancellationToken);
                var clientPublicKey = container.Payload.ReadExactly(stackalloc byte[32]).ToArray();

                Console.WriteLine($"[{DateTime.Now:T}] Server public key: {Convert.ToHexString(serverPublicKey)}");
                Console.WriteLine($"[{DateTime.Now:T}] Client public key: {Convert.ToHexString(clientPublicKey)}");
                Console.WriteLine($"[{DateTime.Now:T}] {direction} => {container}");

                var buffer = container.Payload.ReadToEnd().ToArray();

                // https://github.com/FICTURE7/CoCSharp/blob/d8602264fd185a9236197502eb40aa57019bf4be/src/CoCSharp.Proxy/MessageProcessorNaClProxy.cs#L99

                /// var serverboundCrypto = new Crypto8(Direction.Serverbound, new KeyPair(clientPublicKey, [/*client secret key*/]));
                /// serverboundCrypto.UpdateSharedKey(serverPublicKey);
                /// serverboundCrypto.Decrypt(ref buffer);
                return;
        }

        Console.WriteLine($"[{DateTime.Now:T}] {direction} => {container}");
    }

    private static async Task PumpAsync(SupercellStream source, SupercellStream destination, Direction direction, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var container = await source.ReadContainerAsync(cancellationToken);

            try
            {
                await destination.WriteContainerAsync(container, cancellationToken);

                container.Payload.Position = 0;

                await PacketSentAsync(container, direction, cancellationToken);
            }
            catch (IOException ioException) when (ioException.InnerException is SocketException socketException)
            {
                break;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[{DateTime.Now:T}] Error handling {direction} packet {container}:\n{exception}");
            }
        }
    }

    private static async Task HandleClientAsync(TcpClient client, string upstreamHost, int upstreamPort, CancellationToken cancellationToken = default)
    {
        using var clientConn = client;
        var remote = clientConn.Client.RemoteEndPoint?.ToString() ?? "client";
        Console.WriteLine($"[{DateTime.Now:T}] Incoming connection from {remote}");

        using var upstream = new TcpClient();
        await upstream.ConnectAsync(upstreamHost, upstreamPort, cancellationToken);

        await using var serverboundStream = new SupercellStream(clientConn.GetStream());
        await using var clientboundStream = new SupercellStream(upstream.GetStream());

        try
        {
            await Task.WhenAll(PumpAsync(serverboundStream, clientboundStream, Direction.Serverbound, cancellationToken), PumpAsync(clientboundStream, serverboundStream, Direction.Clientbound, cancellationToken));
        }

        catch (Exception exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remote} closed: {exception.Message}");
        }
    }
}
