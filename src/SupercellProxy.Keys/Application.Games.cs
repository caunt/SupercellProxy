using System.Text.Json;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunGamesAsync(
        string[] args,
        CancellationToken cancellationToken)
    {
        if (args.Any(IsHelp))
            return PrintGamesHelp();

        var positionalArguments = new List<string>(1);
        var outputJson = false;

        foreach (var argument in args)
        {
            if (argument == "--json")
            {
                if (outputJson)
                    throw new ArgumentException("--json may only be specified once.");

                outputJson = true;
            }
            else if (argument.StartsWith("-", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Unknown option: {argument}");
            }
            else
            {
                positionalArguments.Add(argument);
            }
        }

        if (positionalArguments.Count > 1)
            throw new ArgumentException("Usage: SupercellProxy.Keys games [FILE] [--json]");

        var keysPath = Path.GetFullPath(positionalArguments.FirstOrDefault() ?? "KEYS.md");

        if (!File.Exists(keysPath))
            throw new FileNotFoundException("The keys document was not found.", keysPath);

        var document = KeysDocument.Parse(
            await File.ReadAllTextAsync(keysPath, cancellationToken));

        if (outputJson)
        {
            Console.WriteLine(JsonSerializer.Serialize(document.Sections.Select(section => new
            {
                app_id = section.AppStoreId,
                app_name = section.Name
            })));
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
            "List app sections from KEYS.md in document order");
    }
}
