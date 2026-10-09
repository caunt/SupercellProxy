using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Pets;
using SupercellProxy.Networking.Protocol.Sanctuary;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Calls pets to a habitat and collects their pending feeding rewards.</summary>
    public const int CallPetsToHabitatCommandType = 89;
    /// <summary>Calls sanctuary animals to a habitat and collects their pending feeding rewards.</summary>
    public const int CallSanctuaryAnimalsToHabitatCommandType = 217;

    /// <summary>Fills an available pet habitat bowl.</summary>
    public const int FeedPetHabitatCommandType = 86;

    /// <summary>Applies the approved name of a pet or sanctuary animal.</summary>
    public const int RenameAnimalServerCommandType = 245;

    /// <summary>Makes a baby pet without an active care movement run.</summary>
    public const int TapBabyPetCommandType = 340;

    /// <summary>Makes an awake pet run.</summary>
    public const int TapPetCommandType = 88;

    private static void AddPetEntries(Dictionary<int, CommandRegistryEntry> entries)
    {
        entries.Add(
            TapBabyPetCommandType,
            new(
                typeof(TapBabyPetCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => TapBabyPetCommand.Decode(stream, environment)
            )
        );
        entries.Add(
            WakePetCommandType,
            new(
                typeof(WakePetCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => WakePetCommand.Decode(stream, environment)
            )
        );
        entries.Add(
            TapPetCommandType,
            new(
                typeof(TapPetCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => TapPetCommand.Decode(stream, environment)
            )
        );
        entries.Add(
            CallPetsToHabitatCommandType,
            new(
                typeof(CallPetsToHabitatCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => CallPetsToHabitatCommand.Decode(stream, environment)
            )
        );
        entries.Add(
            CallSanctuaryAnimalsToHabitatCommandType,
            new(
                typeof(CallSanctuaryAnimalsToHabitatCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => CallSanctuaryAnimalsToHabitatCommand.Decode(stream, environment)
            )
        );
        entries.Add(
            FeedPetHabitatCommandType,
            new(
                typeof(FeedPetHabitatCommand),
                MessageDirection.Serverbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => FeedPetHabitatCommand.Decode(stream, environment)
            )
        );
        entries.Add(
            RenameAnimalServerCommandType,
            new(
                typeof(RenameAnimalServerCommand),
                MessageDirection.Clientbound,
                BaseFirst: false,
                FieldSchemas: null,
                static (stream, environment, unusedResolver) => RenameAnimalServerCommand.Decode(stream, environment)
            )
        );
    }
}
