using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunGamesAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Any(IsHelp))
            return PrintGamesHelp();

        var positionalArguments = new List<string>(1);
        var outputJson = false;

        foreach (var argument in args)
        {
            if (string.Equals(argument, "--json", StringComparison.Ordinal))
            {
                if (outputJson)
                    throw new ArgumentException("--json may only be specified once.", nameof(args));

                outputJson = true;
            }
            else if (argument.StartsWith('-'))
            {
                throw new ArgumentException($"Unknown option: {argument}", nameof(args));
            }
            else
            {
                positionalArguments.Add(argument);
            }
        }

        if (positionalArguments.Count > 1)
            throw new ArgumentException(
                "Usage: SupercellProxy.Keys games [FILE] [--json]",
                nameof(args)
            );

        var keysPath = Path.GetFullPath(positionalArguments.FirstOrDefault() ?? "KEYS.md");

        if (!File.Exists(keysPath))
            throw new FileNotFoundException("The keys document was not found.", keysPath);

        var document = KeysDocument.Parse(
            await File.ReadAllTextAsync(keysPath, cancellationToken).ConfigureAwait(false)
        );

        if (outputJson)
        {
            Console.WriteLine(
                JsonSerializer.Serialize(
                    document.Sections.Select(static section => new
                    {
                        app_id = section.AppStoreId,
                        app_name = section.Name,
                    })
                )
            );
        }
        else
        {
            foreach (var section in document.Sections)
                Console.WriteLine($"{section.AppStoreId}\t{section.Name}");
        }

        return 0;
    }

    private static int PrintGamesHelp()
    {
        return PrintCommandHelp(
            "games [FILE] [--json]",
            "List app sections from KEYS.md in document order"
        );
    }
}
