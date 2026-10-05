using SupercellProxy.Keys.Extract;
using SupercellProxy.Keys.Extract.Extensions;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunExtractAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
        {
            return PrintCommandHelp(
                usage: "extract INPUT [--key-version]",
                description: "Extract a server public key or its numeric version from a native binary or IPA"
            );
        }

        bool keyVersion = arguments.Length == 2 && arguments[1] == "--key-version";
        RequireOneArgument(keyVersion ? arguments[..1] : arguments, usage: "extract INPUT [--key-version]");

        byte[] binary = await arguments[0]
            .ReadContentAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (binary.HasZipArchiveHeader())
        {
            ReadOnlyMemory<byte> archive = binary;
            binary = await archive
                .GetIpaAppEntryAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        Console.WriteLine(
            keyVersion
            ? KeyVersionExtractor.Extract(binary)?.ToString(System.Globalization.CultureInfo.InvariantCulture)
                ?? throw new InvalidOperationException(message: "Could not find key version in the binary.")
            : Convert.ToHexString(ServerPublicKeyExtractor.ExtractBinary(binary))
        );

        return 0;
    }
}
