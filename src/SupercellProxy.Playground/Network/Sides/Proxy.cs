using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Streams;
using System.Net;
using System.Net.Sockets;

namespace SupercellProxy.Playground.Network.Sides;

// There are multiple ways to implement a proxy that can decrypt packets:
// 1) Get the 'client private key' from the app and give it to the proxy (not sure but maybe 'snonce' needed as well).
// 2) Replace the 'server public key' in the app with 'proxy public key', re-encrypt packets on the proxy.

// CoCSharp.Proxy uses second approach - https://github.com/FICTURE7/CoCSharp/blob/d8602264fd185a9236197502eb40aa57019bf4be/src/CoCSharp.Proxy/MessageProcessorNaClProxy.cs#L99
// They introduced a "standard" key pair for modded servers at Crypto8.StandardKeyPair:
// standard proxy private key: 1891D401FADB51D25D3A9174D472A9F691A45B974285D47729C45C6538070D85
// standard proxy public key: 72F1A4A4C48E44DA0C42310F800E96624E6DC6A641A9D41C3B5039D8DFADC27E

// We are also using second approach, by patching the app's memory to replace the server public key with the standard proxy public key encoded by PublicKeyCodec.Encode()

// Android in-memory public key replacemet:
// 1) Set up Wi-Fi Remote ADB Shell on rooted device
// 2) Download GDB binary for your phone and place it at `/data/local/tmp/gdb` with `adb push`
// 3) Execute `adb shell`, then `su` inside it
// 4) Paste this GDB script in your adb shell:
/*



export PID=$(ps -A | grep 'hayday' | awk '{print $2}')

/data/local/tmp/gdb -q -n -batch -x /dev/stdin <<'EOF'
python
import os, time

processId = os.environ["PID"]
anchorBytes = bytes.fromhex("1AD5000000000000")
replacementBytes = bytes.fromhex("5E2E00002929000047620000DA440000841800003CC400007400000029660000CDA90000A9B10000D4A000001CD40000A076000060E700006EFD0000EC27000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")

def execute_patch():
    overlapSize = len(anchorBytes) - 1

    with open("/proc/%s/maps" % processId, "r") as mapsFile, open("/proc/%s/mem" % processId, "rb+") as memoryFile:
        for currentLine in mapsFile:
            lineParts = currentLine.split()
            if len(lineParts) < 2 or "r" not in lineParts[1]:
            continue

            startString, endString = lineParts[0].split("-")
            currentAddress = int(startString, 16)
            regionEnd = int(endString, 16)

            while currentAddress < regionEnd:
            try:
                    memoryFile.seek(currentAddress)
                    memoryData = memoryFile.read(min(0x4000, regionEnd - currentAddress))
            except IOError:
                break

                if not memoryData or len(memoryData) <= overlapSize:
                break

                matchIndex = memoryData.find(anchorBytes)
                if matchIndex != -1:
                    patchAddress = currentAddress + matchIndex - len(replacementBytes)

                    for retryAttempt in range(5):
                try:
                            memoryFile.seek(patchAddress)
                            memoryFile.write(replacementBytes)
                            memoryFile.flush()
                            print("pattern at 0x%x, patched 0x%x .. 0x%x" % (currentAddress + matchIndex, patchAddress, patchAddress + len(replacementBytes)))
                            return
                        except IOError:
                            time.sleep(0.05)
                    return

                currentAddress += len(memoryData) - overlapSize

execute_patch()
end
EOF



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
            case PassthroughMessage passthroughMessage:
                Console.WriteLine($"[{DateTime.Now:T}] {direction} => {passthroughMessage}");
                var fileName = $"packet_{passthroughMessage.Id}.bin";

                if (!File.Exists(fileName))
                    await File.WriteAllBytesAsync(fileName, passthroughMessage.Data, cancellationToken);

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
        catch (EndOfStreamException exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remote} closed: {exception.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[{DateTime.Now:T}] {remote} closed: {exception}");
        }
    }
}
