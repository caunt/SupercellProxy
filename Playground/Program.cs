using Playground;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

var upstreamHost = args.Length > 0 ? args[0] : "game.haydaygame.com";
var listenPort = args.Length > 1 && int.TryParse(args[1], out var lp) ? lp : 9339;
var upstreamPort = args.Length > 2 && int.TryParse(args[2], out var up) ? up : 9339;

var listener = new TcpListener(IPAddress.Any, listenPort);
listener.Start();
Console.WriteLine($"[{DateTime.Now:T}] Listening on 0.0.0.0:{listenPort}, upstream {upstreamHost}:{upstreamPort}");

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(client, upstreamHost, upstreamPort);
}

// https://github.com/ReversedCell/ScDocumentation/wiki/Encryption-Setup
// https://github.com/ReversedCell/ScDocumentation/wiki/Protocol

static async Task PacketSent(Direction direction, ushort id, ushort version, ScStream stream)
{
    // [00:00:06][serverbound,version=...0,id=10100] Hello from Client
    //     protocolVersion=3
    //     keyVersion=38
    //     clientVersion=1.67.170
    //     fingerprintSha1=be514e02b198d18287af1405089a0e72b849ac69
    //     flags=[1, 1]
    // [00:00:07][clientbound,version=...0,id=20100] length=...28 => 0000001879BF8C72704008FD122900CEBA79E11ADDA2747FBD6A5F7B
    // [00:00:07][serverbound,version=3242,id=10101] length=..861 => 3A32AF44BC6BF8307FC0CD9C8295A5B547FF473A91F40DF77BC52E2BE74A2256F08731815CBB2AC69AF1EEE370C6BC3407A59586BC4196D03CAFC447F734D555...
    // [00:00:07][clientbound,version=...0,id=25220] length=..915 => 3BB5D82E064BEAC0379DE6DB2AF3EE6FE50F0D356B925F69F86401EFDCC2D29DE28A51FBBC067471F9434CA6C5712E1AD97A89CD2477A47AE39EA540D71C8710...
    // [00:00:07][clientbound,version=3242,id=20155] length=...17 => 127F8252442D1CE9ADA5290305D2A7AD24
    // [00:00:07][serverbound,version=3242,id=10964] length=...73 => 8305FD7749B1E5CB5E7494FAEF1C9520FAEA7C0D703E628EA82F18FF061D52E4C3EA9F3E918FE0F76C3C3D9E0F80A8E0480B8050B770E7A82AD0563379C800B8...
    // [00:00:07][serverbound,version=3242,id=17339] length=.1247 => C011A25C6B1AECC18B8F0C65690FE08E534F2C8CEF076BDDF1CB47B8629C8F697B4C41D84EF1AC6993A5B41151421D310EC768EB38A06339A5B8803684467BBE...
    // [00:00:07][serverbound,version=3242,id=19949] length=...17 => A1261722659105CA08D60C5449A1395E7F
    // [00:00:07][serverbound,version=3242,id=10964] length=...55 => 87FF56947BE39DD9B95F4E50914C23338A4742FEA3CA42A86F62BE2A7830D1B885A96BBDC7DFE1E1BDF80D32498B6E879D8162E0028924
    // [00:00:07][clientbound,version=3242,id=20187] length=43037 => 00A04A59B74918E3320E8F00E1D516995B8380DAF96E272607EC5D4E6E3DF25B838670F4646CEDC4893C0D86C0CAB98246388487D90B10957897035B7FF76DF1...
    // [00:00:08][clientbound,version=...0,id=24180] length=52828 => 3FCCEFB6252513C0FC76E128FAA4E58EBFBF7809CFA4B99FE3FA0F0E9E47BEF2EC7C79E43DFB80A7192D787A232D507EDD2CD7B4C79213E81536639745D3023A...
    // [00:00:08][clientbound,version=3242,id=20477] length=...18 => BF1CB39EEFAB12D38971BB45A97075FBF50F
    // [00:00:08][clientbound,version=3242,id=28061] length=..521 => C0F3D4FAAD1ACF446B08BD03D8D8C1D3585B77A42E2FF2971BB445699E616C1AF315E8CAFE72230728EA38077E5AC5F03D2A17A711ECD13A0402C53691FC6117...
    // [00:00:08][clientbound,version=3242,id=20621] length=...17 => 85DDD5FAB14D864757F96F99F7966FA30D
    // [00:00:08][serverbound,version=3242,id=17339] length=..761 => DC92F43E9120AFDEEA9A84D82285DD579F9818E24DDCB1D4646AA43E1A95B14C1320E8EB5580F59CB197C02610E5D385759A193CF439D9E3C56D3A061525AF86...
    // [00:00:08][clientbound,version=3242,id=20187] length=43037 => 8A6879C1B282E1DB619FE063355574D470D76DDDD5C726B5B5AE48CB1FF16A42DF330D56EBBC736A9D29205A7597A9629B6A65AE8820844520FE663F18CFB837...
    // [00:00:08][clientbound,version=3242,id=20187] length=...29 => 3A48E641E622C468E45E78658A0C340A7B12E1E833748E5F18C154F80C
    // [00:00:08][clientbound,version=3242,id=20187] length=14080 => FC4CE7E12C8CA203EAEE92AA023862FD6EDA66E3EF50303E849485EB973F5C4D80750AC963DD9CB2D6AC2764463549774FD00B149DFEF1413EB56174E2B4386B...
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => 154383FBC144FF75314A10757FB106050DF04D80A911EE8A1B4206191EE7A8F0ADE70F3385ADBF41836A7200
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => 8D9DF91D6FDBF407EB6F666BE3D0B9E23A8264C993C9C557B62D8B37A305D7AF6C6638F2A57C3D63AD0F9F80
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => D4450C1041FF12E324F097FBF4B69B4A77EA597DD1D4F91FACB94F61718B9D0698A9B10EF0C14BDC2BF7D7F7
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => 238FA03732FD50AFA52428030D84D448532CA4936240D089EBF7A5E20DC9A7A7E081BFDBE41D843B904E89FB
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => 274CB0C774A4068A90638631CCE0739913097F8363E5294CC52D1D4107C8DECAAE910191E850FD58188D2FAF
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => FF521956F5AB3B1B968FB5BF056DE2F9A43E0F1458F009CE82CCD1629F4B0E1C28FFEA6CF9A9873FF49180B9
    // [00:00:08][serverbound,version=3242,id=10964] length=...44 => F9118E4D3554AEA6CEA7D0FF36E43A082E0BE3D3CE73615BF704531C485DDD9495EA27B9C1BB0C2BD1E4AFF3
    // [00:00:08][serverbound,version=3242,id=16886] length=...32 => 098EEAF3D6FD8AFC53A400F2F0BEBC2127E83FF08B5252CD4B6AB105B756586E
    // [00:00:08][serverbound,version=3242,id=11841] length=...42 => 248C4AE9557096FE500B5798506D8328E071BD89BFD2C8E9A266E885233D446B3438CFDE7ECBF28AC1F4
    // [00:00:08][serverbound,version=3242,id=18386] length=...16 => 28E227A3708258633A2F59A09666EDDA

    var prefix = $"[{DateTime.Now:T}][{direction.ToString().ToLower()},version={PadLeft(version, 4)},id={PadLeft(id, 5)}]";

    PacketReader reader = (direction, id) switch
    {
        (Direction.Serverbound, 10100) => ReadLoginPacket,
        (Direction.Clientbound, 20103) => ReadLoginFailedPacket,
        _ => ReadUnknownPacket
    };

    reader(prefix, stream);
}

static void ReadLoginPacket(string prefix, ScStream stream)
{
    var protocolVersion = stream.ReadInt32();
    var keyVersion = stream.ReadInt32();

    var clientVersionMajor = stream.ReadInt32();
    var clientVersionMinor = stream.ReadInt32();
    var clientVersionPatch = stream.ReadInt32();

    var fingerprintSha1 = stream.ReadString();

    var flag1 = stream.ReadInt32();
    var flag2 = stream.ReadInt32();
    Console.WriteLine($"{prefix} Hello from Client" +
        $"\n\tprotocolVersion={protocolVersion}" +
        $"\n\tkeyVersion={keyVersion}" +
        $"\n\tclientVersion={clientVersionMajor}.{clientVersionMinor}.{clientVersionPatch}" +
        $"\n\tfingerprintSha1={fingerprintSha1}" +
        $"\n\tflags=[{flag1}, {flag2}]");
}

static void ReadLoginFailedPacket(string prefix, ScStream stream)
{
    // 8 - UpdateRequired
    var errorCode = stream.ReadInt32();
    var resourceFingerprintData = stream.ReadString();
    var redirectDomain = stream.ReadString();
    var contentURL = stream.ReadString();
    var updateURL = stream.ReadString();
    var reason = stream.ReadString();
    var secondsUntilMaintenanceEnd = stream.ReadInt32();
    var unknown1 = stream.ReadByte();
    var unknown2 = stream.ReadString();
    var unknown3 = stream.ReadString();
    var unknown4 = stream.ReadInt32();
    var unknown5 = stream.ReadInt32();
    var unknown6 = stream.ReadString();
    var unknown7 = stream.ReadString();

    var unknown8 = new int[BinaryPrimitives.ReverseEndianness(stream.ReadUInt16())];
    for (var i = 0; i < unknown8.Length; i++)
        unknown8[i] = stream.ReadInt32();

    var unknown9 = stream.ReadByte();

    Console.WriteLine($"{prefix} LoginFailed from server" +
        $"\n\terrorCode={errorCode}" +
        $"\n\tresourceFingerprintData={resourceFingerprintData}" +
        $"\n\tredirectDomain={redirectDomain}" +
        $"\n\tcontentURL={contentURL}" +
        $"\n\tupdateURL={updateURL}" +
        $"\n\treason={reason}" +
        $"\n\tsecondsUntilMaintenanceEnd={secondsUntilMaintenanceEnd}" +
        $"\n\tunknown1={unknown1}" +
        $"\n\tunknown2={unknown2}" +
        $"\n\tunknown3={unknown3}" +
        $"\n\tunknown4={unknown4}" +
        $"\n\tunknown5={unknown5}" +
        $"\n\tunknown6={unknown6}" +
        $"\n\tunknown7={unknown7}" +
        $"\n\tunknown8=[{string.Join(", ", unknown8)}]" +
        $"\n\tunknown9={unknown9}");
}

static async Task PumpAsync(NetworkStream source, NetworkStream destination, Direction direction)
{
    var header = new byte[7];

    while (true)
    {
        await source.ReadExactlyAsync(header);

        var span = header.AsSpan();

        var id = BinaryPrimitives.ReadUInt16BigEndian(span[0..2]);
        var length = (span[2] << 16) | (span[3] << 8) | span[4];
        var version = BinaryPrimitives.ReadUInt16BigEndian(span[5..7]);

        await destination.WriteAsync(header);
        var payload = new byte[length];

        if (length > 0)
        {
            await source.ReadExactlyAsync(payload);
            await destination.WriteAsync(payload);
        }

        try
        {
            using var stream = new ScStream(payload);
            await PacketSent(direction, id, version, stream);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] Error handling {direction} packet id={id} length={length} version={version}:\n{exception}");
        }
    }
}

static async Task HandleClientAsync(TcpClient client, string upstreamHost, int upstreamPort)
{
    using var clientConn = client;
    var remote = clientConn.Client.RemoteEndPoint?.ToString() ?? "client";
    Console.WriteLine($"[{DateTime.Now:T}] Incoming connection from {remote}");

    using var upstream = new TcpClient();
    await upstream.ConnectAsync(upstreamHost, upstreamPort);

    using var c2s = clientConn.GetStream();
    using var s2c = upstream.GetStream();

    try
    {
        await Task.WhenAll(PumpAsync(c2s, s2c, Direction.Serverbound), PumpAsync(s2c, c2s, Direction.Clientbound));
    }
    catch (Exception exception)
    {
        Console.WriteLine($"[{DateTime.Now:T}] {remote} closed: {exception.Message}");
    }
}

static void ReadUnknownPacket(string prefix, ScStream stream)
{
    const int maxOutputWidth = 64;

    var data = stream.ReadToEnd();
    var sliced = data.Length > maxOutputWidth;
    var preview = Convert.ToHexString(data[..Math.Min(maxOutputWidth, data.Length)]);

    if (sliced)
        preview += "...";

    Console.WriteLine($"{prefix} length={PadLeft(stream.Length, 5)} => {preview}");
}

static string? PadLeft<T>(T value, int width, char @char = '.') where T : struct => value.ToString()?.PadLeft(width, @char);

delegate void PacketReader(string prefix, ScStream stream);

enum Direction
{
    Clientbound,
    Serverbound
}
