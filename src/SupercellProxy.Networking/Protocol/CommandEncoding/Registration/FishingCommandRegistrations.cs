using SupercellProxy.Networking.Protocol.Fishing;
using SupercellProxy.Networking.Protocol.Fishing.Molluscs;
using SupercellProxy.Networking.Protocol.MessageEncoding;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class FishingCommandRegistrations
{
    internal static readonly Dictionary<int, CommandRegistryEntry> Entries = new()
    {
        [CollectMolluscCommandType] = new CommandRegistryEntry(
            typeof(CollectMolluscCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter) => CollectMolluscCommand.Decode(stream, environment)
        ),
        [OpenMolluscCommandType] = new CommandRegistryEntry(
            typeof(OpenMolluscCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter) => OpenMolluscCommand.Decode(stream, environment)
        ),
        [CollectAngusPearlsCommandType] = new CommandRegistryEntry(
            typeof(Characters.CollectAngusPearlsCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter) => Characters.CollectAngusPearlsCommand.Decode(stream, environment)
        ),
        [SetFishStateCommandType] = new CommandRegistryEntry(
            typeof(SetFishStateCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                SetFishStateCommand.Decode(stream, environment)
        ),
        [CollectFishingSpotCommandType] = new CommandRegistryEntry(
            typeof(CollectFishingSpotCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectFishingSpotCommand.Decode(stream, environment)
        ),
        [CollectFishingTrapCommandType] = new CommandRegistryEntry(
            typeof(CollectFishingTrapCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter) => CollectFishingTrapCommand.Decode(stream, environment)
        ),
        [CollectLobsterCommandType] = new CommandRegistryEntry(
            typeof(CollectFishingAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter) => CollectFishingAnimalCommand.Decode(stream, environment, duck: false)
        ),
        [CollectDuckCommandType] = new CommandRegistryEntry(
            typeof(CollectFishingAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter) => CollectFishingAnimalCommand.Decode(stream, environment, duck: true)
        ),
        [CompleteFishingCatchCommandType] = new CommandRegistryEntry(
            typeof(CompleteFishingCatchCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CompleteFishingCatchCommand.Decode(stream, environment)
        ),
        [PlaceFishingBaitCommandType] = new CommandRegistryEntry(
            typeof(PlaceFishingBaitCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PlaceFishingBaitCommand.Decode(stream, environment)
        ),
        [PlaceFishingNetCommandType] = new CommandRegistryEntry(
            typeof(PlaceFishingNetCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PlaceFishingNetCommand.Decode(stream, environment)
        ),
        [RemoveDuckCommandType] = new CommandRegistryEntry(
            typeof(RemoveFishingAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) => RemoveFishingAnimalCommand.Decode(stream, environment, duck: true)
        ),
        [RemoveLobsterCommandType] = new CommandRegistryEntry(
            typeof(RemoveFishingAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) => RemoveFishingAnimalCommand.Decode(stream, environment, duck: false)
        ),
    };
}
