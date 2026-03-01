using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
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

// Android in-memory public key replacemet:
// 1) Set up Wi-Fi Remote ADB Shell on rooted device
// 2) Download GDB binary for your phone and place it at `/data/local/tmp/gdb` with `adb push`
// 3) Execute `adb shell`, then `su` inside it
// 4) Paste this GDB script in your adb shell:
/*
cat > /data/local/tmp/pattern_patch.gdb <<'EOF'
python
import os

def hex_to_bytes(hex_string):
    hex_string = hex_string.replace(" ", "").replace("\n", "")
    return bytes(bytearray(int(hex_string[i:i + 2], 16) for i in range(0, len(hex_string), 2)))

pid = int (os.environ["PID"])

anchor = hex_to_bytes("1AD5000000000000")

replacement = hex_to_bytes("5E2E00002929000047620000DA440000841800003CC400007400000029660000CDA90000A9B10000D4A000001CD40000A076000060E700006EFD0000EC27000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")

chunk_size = 0x4000

def patch_pattern_in_process(pattern, pattern_replacement, offset= 0, limit= None):
    if not pattern:
        return 0

    maps_path = "/proc/%d/maps" % pid
    mem_path = "/proc/%d/mem" % pid

    try:
        maps_file = open(maps_path, "r")
    except IOError as error:
        print("cannot open maps:", error)
        return 0

    try:
        mem_file = open(mem_path, "rb+")
    except IOError as error:
        maps_file.close()
        print("cannot open mem:", error)
        return 0

    overlap = len(pattern) - 1 if len(pattern) > 1 else 0
    patched_count = 0

    for line in maps_file:
        parts = line.split()
        if len(parts) < 2:
            continue

        addr_range = parts[0]
        perms = parts[1]

        if "r" not in perms:
            continue

        start_str, end_str = addr_range.split("-")
        region_start = int (start_str, 16)
        region_end = int (end_str, 16)

        current_address = region_start

        while current_address<region_end:
            size = min(chunk_size, region_end - current_address)

            try:
                mem_file.seek(current_address)
                data = mem_file.read(size)
            except IOError:
                break

            if not data:
                break

            search_offset = 0

            while True:
                index = data.find(pattern, search_offset)
                if index == -1:
                    break

                found_address = current_address + index
                patch_start = found_address + offset

                if patch_start< 0:
                    print("found pattern at 0x%x but patch_start < 0" % found_address)
                    maps_file.close()
                    mem_file.close()
                    return patched_count

                try:
                    mem_file.seek(patch_start)
                    mem_file.write(pattern_replacement)
                    patched_count += 1
                    print("pattern at 0x%x, patched 0x%x .. 0x%x"
                          % (found_address, patch_start, patch_start + len(pattern_replacement)))
                except IOError as error:
                    print("write failed:", error)
                    maps_file.close()
                    mem_file.close()
                    return patched_count

                if limit is not None and patched_count >= limit:
                    maps_file.close()
                    mem_file.close()
                    return patched_count

                search_offset = index + 1

            if len(data) <= overlap:
                break

            current_address += len(data) - overlap

    if patched_count == 0:
        print("pattern not found")

    maps_file.close()
    mem_file.close()
    return patched_count


def patch_before_anchor():
    # patch len(replacement) bytes immediately before the anchor, only once
    patch_pattern_in_process(anchor, replacement, offset= -len(replacement), limit= 1)


patch_before_anchor()
end
EOF

export PID =$(ps -A | grep 'hayday' | awk '{print $2}')
export TRACER_PID =$(grep TracerPid /proc/$PID/status | awk '{print $2}')
/data/local/tmp/gdb -q -n -batch \
    -ex "source /data/local/tmp/pattern_patch.gdb" \
    /system/bin/app_process32 $TRACER_PID

*/

public record ProxyConfiguration(string UpstreamHost, int UpstreamPort, string ListenAddress, int ListenPort, ProtocolConfiguration Protocol);

public class Proxy(ProxyConfiguration configuration)
{
    public static readonly byte[] StandardPrivateKey = [0x18, 0x91, 0xD4, 0x01, 0xFA, 0xDB, 0x51, 0xD2, 0x5D, 0x3A, 0x91, 0x74, 0xD4, 0x72, 0xA9, 0xF6, 0x91, 0xA4, 0x5B, 0x97, 0x42, 0x85, 0xD4, 0x77, 0x29, 0xC4, 0x5C, 0x65, 0x38, 0x07, 0x0D, 0x85];

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

    private static async ValueTask<bool> HandleMessageReceivedAsync(IMessage message, Direction direction, SupercellStream source, SupercellStream destination, CancellationToken cancellationToken = default)
    {
        switch (message)
        {
            case ClientHelloMessage clientHelloMessage:
                Console.WriteLine($"[{DateTime.Now:T}] {clientHelloMessage}");
                return false;
            case ServerHelloMessage serverHelloMessage:
                Console.WriteLine($"[{DateTime.Now:T}] {serverHelloMessage}");
                await source.SetupEncryptionAsync(Side.Server, serverHelloMessage.SessionKey, cancellationToken);
                return false;
            case LoginMessage loginMessage:
                Console.WriteLine($"[{DateTime.Now:T}] {loginMessage}");
                return false;
        }

        Console.WriteLine($"[{DateTime.Now:T}] {direction} => {message}");

        return false;
    }

    private static async ValueTask HandleMessageSentAsync(IMessage message, Direction direction, SupercellStream source, SupercellStream destination, CancellationToken cancellationToken = default)
    {
        switch (message)
        {
            case ServerHelloMessage serverHelloMessage:
                await destination.SetupEncryptionAsync(Side.Client, serverHelloMessage.SessionKey, cancellationToken);
                return;
        }
    }

    private static async Task PumpAsync(SupercellStream source, SupercellStream destination, Direction direction, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await source.ReadMessageAsync(cancellationToken);
            var cancelled = await HandleMessageReceivedAsync(message, direction, source, destination, cancellationToken);

            if (cancelled)
                continue;

            try
            {
                await destination.WriteMessageAsync(message, cancellationToken);
                await HandleMessageSentAsync(message, direction, source, destination, cancellationToken);
            }
            catch (IOException ioException) when (ioException.InnerException is SocketException socketException)
            {
                break;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[{DateTime.Now:T}] Error handling {direction} packet {message}:\n{exception}");
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
            var serverboundPumpTask = PumpAsync(serverboundStream, clientboundStream, Direction.Serverbound, cancellationToken);
            var clientboundPumpTask = PumpAsync(clientboundStream, serverboundStream, Direction.Clientbound, cancellationToken);

            var completedTask = await Task.WhenAny(serverboundPumpTask, clientboundPumpTask);
            await completedTask; // propagate exceptions

            if (completedTask == serverboundPumpTask)
                await clientboundPumpTask;
            else
                await serverboundPumpTask;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remote} closed: {exception}");
        }
    }
}
