using SupercellProxy.Playground.Network.Sides.Configuration;
using SupercellProxy.Playground.Network.Sides.Proxy;

var upstreamHost = args.Length > 0 ? args[0] : "game.haydaygame.com";
var upstreamPort = args.Length > 1 && int.TryParse(args[1], out var up) ? up : 9339;

// var client = new ScClient(new ClientConfiguration(
//     UpstreamHost: upstreamHost,
//     UpstreamPort: upstreamPort,
//     Protocol: new ProtocolConfiguration(
//         MajorVersion: 1,
//         MinorVersion: 69,
//         PatchVersion: 89,
//         ProtocolVersion: 3,
//         KeyVersion: 40)));
// 
// await client.RunAsync();

// static bool LooksLikeZlibHeader(byte cmf, byte flg)
// {
//     if (cmf != 0x78)
//     {
//         return false;
//     }
// 
//     var header = (cmf << 8) | flg;
//     return header % 31 == 0;
// }
// 
// var binary = await File.ReadAllBytesAsync("other_home_data_message.bin");
// 
// Directory.CreateDirectory("out");
// 
// var offset = 0;
// var index = 0;
// 
// while (offset + 6 <= binary.Length)
// {
//     var decompressedLength = BinaryPrimitives.ReadInt32LittleEndian(binary.AsSpan(offset, 4));
// 
//     if (decompressedLength <= 0 || decompressedLength > 128 * 1024 * 1024)
//     {
//         offset++;
//         continue;
//     }
// 
//     var cmf = binary[offset + 4];
//     var flg = binary[offset + 5];
// 
//     if (!LooksLikeZlibHeader(cmf, flg))
//     {
//         offset++;
//         continue;
//     }
// 
//     try
//     {
//         using var input = new MemoryStream(binary, offset + 4, binary.Length - (offset + 4), writable: false);
//         using var zlib = new ZLibStream(input, CompressionMode.Decompress);
// 
//         var output = new byte[decompressedLength];
//         var outputOffset = 0;
// 
//         while (outputOffset < output.Length)
//         {
//             var bytesRead = zlib.Read(output, outputOffset, output.Length - outputOffset);
// 
//             if (bytesRead == 0)
//             {
//                 throw new InvalidDataException("Unexpected end of zlib stream.");
//             }
// 
//             outputOffset += bytesRead;
//         }
// 
//         await File.WriteAllBytesAsync(Path.Combine("out", $"{index:D5}.bin"), output);
// 
//         offset += 4 + (int)input.Position;
//         index++;
//     }
//     catch
//     {
//         offset++;
//     }
// }
// 
// Console.WriteLine(index);
// 
// return;

var listenAddress = args.Length > 2 ? args[2] : "0.0.0.0";
var listenPort = args.Length > 3 && int.TryParse(args[3], out var lp) ? lp : 9339;

var proxy = new ScProxy(new ProxyConfiguration(
    UpstreamHost: upstreamHost,
    UpstreamPort: upstreamPort,
    ListenAddress: listenAddress,
    ListenPort: listenPort,
    Protocol: new ProtocolConfiguration(
        MajorVersion: 1,
        MinorVersion: 69,
        PatchVersion: 89,
        ProtocolVersion: 3,
        KeyVersion: 40)));

await proxy.RunAsync();

// var server = new ScServer(listenAddress, listenPort);
// await server.RunAsync();