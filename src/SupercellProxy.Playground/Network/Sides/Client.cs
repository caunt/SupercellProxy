using SupercellProxy.Playground.Crypto;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace SupercellProxy.Playground.Network.Sides;

public partial class Client(string upstreamHost, int upstreamPort)
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var upstream = new TcpClient();
            await upstream.ConnectAsync(upstreamHost, upstreamPort, cancellationToken);

            await using var networkStream = upstream.GetStream();
            await using var supercellStream = new SupercellStream(networkStream);

            await supercellStream.WriteMessageAsync(new ClientHelloMessage
            {
                ProtocolVersion = 3,
                KeyVersion = 40,

                MajorVersion = 1,
                MinorVersion = 69,
                PatchVersion = 89,

                // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
                // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
                // 1.69.89  => 0c95746ec8ced89978f4b9fded2fdbc95b3daf18
                FingerprintSha1 = "0c95746ec8ced89978f4b9fded2fdbc95b3daf18",

                DeviceType = 1,
                AppStore = 1
            }.ToContainer(), cancellationToken);

            var serverHello = ServerHelloMessage.Create(await supercellStream.ReadMessageAsync(cancellationToken));

            // 10101
            var loginMessage = new LoginMessage
            {
                AccountId = 0,
                PassToken = "",
                ResourceSha = "",
                LoginVersion = 0,
                UdId = "",
                OpenUdId = "",
                MacAddress = "",
                DeviceModel = "",
                AdId = "",
                IsAdTracking = false,
                OsVersion = "",
                Locale = "",
                Idfv = "",
                PreferredLanguage = "",
                ScidString = "",
                UnknownBool = false,
                ScIdToken = "",
                UnknownInt = 0,
                DataRef = 0,
                SystemString1 = "",
                SystemString2 = ""
            };

            var serverPublicKey = await HayDayApi.GetServerPublicKeyAsync(cancellationToken);
            var clientPrivateKey = RandomNumberGenerator.GetBytes(count: 32);
            var clientPublicKey = NaClV3Crypto.CryptoScalarMultBase(clientPrivateKey);

            var decryptNonce = new Nonce(clientPublicKey: clientPublicKey, serverPublicKey: serverPublicKey);
            var nonce = new Nonce(clientPublicKey: clientPublicKey, serverPublicKey: serverPublicKey);

            var encrypted = NaClV3Crypto.Box([.. serverHello.SessionKey.Span, .. decryptNonce.Span, .. loginMessage.ToContainer().Payload.ToArray(), .. stackalloc byte[508]], nonce.Span, serverPublicKey, clientPrivateKey);
            await supercellStream.WriteMessageAsync(new MessageContainer(10101, 5209, new SupercellStream(new MemoryStream([.. clientPublicKey, .. encrypted]))), cancellationToken);

            // any
            var anyContainer = await supercellStream.ReadMessageAsync(cancellationToken);
            Console.WriteLine(anyContainer);
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Connection closed by remote host.");
        }
    }
}