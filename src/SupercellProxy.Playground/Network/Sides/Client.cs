using Blake2Fast;
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

            // 10100
            var clientHelloContainer = CreateClientHello().ToContainer();
            Console.WriteLine(clientHelloContainer);
            await supercellStream.WriteMessageAsync(clientHelloContainer, cancellationToken);

            // 20100
            var serverHelloContainer = await supercellStream.ReadMessageAsync(cancellationToken);
            var serverHello = ServerHelloMessage.Create(serverHelloContainer);
            Console.WriteLine(serverHelloContainer);

            // 10101
            var serverPublicKey = await HayDayApi.GetServerPublicKeyAsync(cancellationToken);
            var loginContainer = CreateLoginMessageContainer(serverPublicKey, serverHello.SessionKey.Span);
            Console.WriteLine(loginContainer);
            await supercellStream.WriteMessageAsync(loginContainer, cancellationToken);

            // any
            var anyContainer = await supercellStream.ReadMessageAsync(cancellationToken);
            Console.WriteLine(anyContainer);
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Connection closed by remote host.");
        }
    }

    private static MessageContainer CreateLoginMessageContainer(Span<byte> serverPublicKey, Span<byte> sessionToken)
    {
        var loginMessageStream = CreateLoginMessage().ToContainer();

        var loginMessageBuffer = loginMessageStream.Payload.ToArray();

        var clientPrivateKey = RandomNumberGenerator.GetBytes(count: 32);
        var clientPublicKey = NaClV3Crypto.CryptoScalarMultBase(clientPrivateKey);
        var clientNonce = RandomNumberGenerator.GetBytes(count: 24);
        clientNonce[0] &= 0xFE;

        var hasher = Blake2b.CreateIncrementalHasher(digestLength: 24);
        hasher.Update(clientPublicKey);
        hasher.Update(serverPublicKey);
        var tempNonce = hasher.Finish();

        var encrypted = NaClV3Crypto.Box([.. sessionToken, .. clientNonce, .. loginMessageBuffer, .. new byte[508]], tempNonce, serverPublicKey.ToArray(), clientPrivateKey);

        return new MessageContainer(10101, 5209, new SupercellStream(new MemoryStream([.. clientPublicKey, .. encrypted])));

        // FF9C8D567D78F6DE 1BDB27F7B4E4EE8D3F359292149F5EF3C46D59C1404DC91D
        // 8602D5C784329E3E 9A763F079F247156C1649B2D0E36B1C73CCB3AE5FE3CEC54A8CE22FA4D2C026730BCE46EAE4205DE0ED6432B0D0BF8C2052F884CF1650E37F5C20E65C2AC882F4AC1F80BC00743D6CAD10542606DE4BCD92D022C713D22CE3C32EC3C2BBC3ACBC258B136BFAF9B64C80C7124DB983F7684309E32BD3ED502
    }

    private static LoginMessage CreateLoginMessage()
    {
        return new LoginMessage
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
    }

    private static ClientHelloMessage CreateClientHello()
    {
        return new ClientHelloMessage
        {
            ProtocolVersion = 3,
            KeyVersion = 40,

            MajorVersion = 1,
            MinorVersion = 69,
            PatchVersion = 89,

            // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
            // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
            FingerprintSha1 = "0c95746ec8ced89978f4b9fded2fdbc95b3daf18",

            DeviceType = 1,
            AppStore = 1

            // Squad Busters: ProtocolVersion = 1,
            // Squad Busters: KeyVersion = 57,
            // Squad Busters: 
            // Squad Busters: MajorVersion = 13,
            // Squad Busters: MinorVersion = 807,
            // Squad Busters: PatchVersion = 7,
            // Squad Busters: 
            // Squad Busters: FingerprintSha1 = "a0bcd279dbf934648bca39d48cd609e65623a9d9",
            // Squad Busters: 
            // Squad Busters: DeviceType = 1,
            // Squad Busters: AppStore = 1
        };
    }
}