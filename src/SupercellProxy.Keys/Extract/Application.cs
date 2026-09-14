using SupercellProxy.Keys.Extract;
using SupercellProxy.Keys.Extract.Extensions;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunExtractAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
            return PrintCommandHelp(usage: "extract INPUT", description: "Extract a server public key from a native binary or IPA");

        RequireOneArgument(arguments, usage: "extract INPUT");

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

        Console.WriteLine(Convert.ToHexString(ServerPublicKeyExtractor.ExtractBinary(binary)));

        return 0;
    }
}
