using SupercellProxy.PublicKeyExtractor;
using SupercellProxy.PublicKeyExtractor.Extensions;

byte[] binary;

if (args.Length < 1)
{
    Console.WriteLine(
        "Please provide the path or URL to IPA file or libg.so dump.\nAPK files are not supported."
    );
    return 1;
}

try
{
    var input = args[0];
    binary = await input.ReadContentAsync(CancellationToken.None).ConfigureAwait(false);
}
catch (Exception exception)
{
    Console.WriteLine($"Could not read content: {exception.Message}");
    return 2;
}

try
{
    if (binary.HasZipArchiveHeader()) // Only for .ipa files
    {
        ReadOnlyMemory<byte> binaryMemory = binary;
        binary = await binaryMemory
            .GetIpaAppEntryAsync(CancellationToken.None)
            .ConfigureAwait(false);
    }
}
catch (Exception exception)
{
    Console.WriteLine($"Could not get binary from IPA: {exception.Message}");
    return 3;
}

try
{
    var serverPublicKey = ServerPublicKeyExtractor.ExtractBinary(binary);
    Console.WriteLine(Convert.ToHexString(serverPublicKey));
}
catch (Exception exception)
{
    Console.WriteLine($"Could not extract server public key:\n{exception}");
    return 4;
}

return 0;
