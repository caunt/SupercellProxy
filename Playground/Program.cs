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
    var prefix = $"[{DateTime.Now:T}][{direction.ToString().ToLower()},version={PadLeft(version, 4)},id={PadLeft(id, 5)}]";

    PacketReader? reader = (direction, id) switch
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
