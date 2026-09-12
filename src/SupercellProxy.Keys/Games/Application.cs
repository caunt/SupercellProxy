using System.Text.Json;

using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal static partial class Application
{

    private static int PrintGamesHelp()
    {
        return PrintCommandHelp(usage: "games [FILE] [--json]", description: "List app sections from KEYS.md in document order");
    }

    private static async Task<int> RunGamesAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
            return PrintGamesHelp();

        List<string> positionalArguments = new(capacity: 1);
        bool outputDocument = false;

        foreach (string argument in arguments)
        {
            if (string.Equals(argument, b: "--json", StringComparison.Ordinal))
            {
                if (outputDocument)
                    throw new ArgumentException(message: "--json may only be specified once.", nameof(arguments));

                outputDocument = true;
            }
            else if (argument.StartsWith(value: '-'))
            {
                throw new ArgumentException($"Unknown option: {argument}", nameof(arguments));
            }
            else
            {
                positionalArguments.Add(argument);
            }
        }

        if (positionalArguments.Count > 1)
            throw new ArgumentException(message: "Usage: SupercellProxy.Keys games [FILE] [--json]", nameof(arguments));

        string keysPath = Path.GetFullPath(positionalArguments.FirstOrDefault() ?? "KEYS.md");

        if (!File.Exists(keysPath))
            throw new FileNotFoundException(message: "The keys document was not found.", keysPath);

        KeysDocument document = KeysDocument.Parse(await File.ReadAllTextAsync(keysPath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));

        if (outputDocument)
        {
            Console.WriteLine(
                JsonSerializer.Serialize(document.Sections.Select(static section => new { app_id = section.AppStoreIdentifier, app_name = section.Name, }))
            );
        }
        else
        {
            foreach (KeysSection section in document.Sections)
                Console.WriteLine($"{section.AppStoreIdentifier}\t{section.Name}");
        }

        return 0;
    }
}
