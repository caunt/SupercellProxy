using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Requests help for a crate on the player's boat.</summary>
    public const int RequestBoatCrateHelpCommandType = 91;

    /// <summary>Accepts or declines the expired boat's time-extension offer.</summary>
    public const int RespondToBoatTimeExtensionCommandType = 712;

    private static void AddBoatEntries(Dictionary<int, CommandRegistryEntry> entries)
    {
        entries.Add(
            RequestBoatCrateHelpCommandType,
            new(
                typeof(RequestBoatCrateHelpCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => RequestBoatCrateHelpCommand.Decode(stream, environment)
            )
        );

        entries.Add(
            RespondToBoatTimeExtensionCommandType,
            new(
                typeof(RespondToBoatTimeExtensionCommand),
                MessageDirection.Serverbound,
                BaseFirst: true,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => RespondToBoatTimeExtensionCommand.Decode(stream, environment)
            )
        );
    }
}
