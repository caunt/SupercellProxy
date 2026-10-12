using SupercellProxy.Networking.Protocol.Expansion;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Unlocks land using the expansion's required materials.</summary>
    public const int ExpandLandCommandType = 90;

    private static void AddExpansionEntries(Dictionary<int, CommandRegistryEntry> entries)
    {
        entries.Add(
            ExpandLandCommandType,
            new(
                typeof(ExpandLandCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => ExpandLandCommand.Decode(stream, environment)
            )
        );
    }
}
