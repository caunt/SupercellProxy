using System.Globalization;
using System.Resources;

namespace SupercellProxy.PublicKeyExtractor;

internal static class ApplicationText
{
    private static readonly ResourceManager ResourceManager = new(
        "SupercellProxy.PublicKeyExtractor.ApplicationText",
        typeof(ApplicationText).Assembly
    );

    public static string InputRequired =>
        ResourceManager.GetString(nameof(InputRequired), CultureInfo.CurrentUICulture)
        ?? throw new MissingManifestResourceException(
            $"The {nameof(InputRequired)} resource is missing."
        );
}
