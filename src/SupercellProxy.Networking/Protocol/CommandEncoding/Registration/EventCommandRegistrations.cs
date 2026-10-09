using SupercellProxy.Networking.Protocol.ShopEvents;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Events.Chronos;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class EventCommandRegistrations
{
    internal static readonly Dictionary<int, CommandRegistryEntry> Entries = new()
    {
        [AcknowledgeEventSequenceCommandType] = new(
            typeof(AcknowledgeEventSequenceCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => AcknowledgeEventSequenceCommand.Decode(stream, environment)
        ),
        [ClaimNeighborhoodObjectVisualRewardCommandType] = new(
            typeof(ClaimNeighborhoodObjectVisualRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ClaimNeighborhoodObjectVisualRewardCommand.Decode(stream, environment)
        ),
        [ClaimNeighborhoodObjectRewardCommandType] = new(
            typeof(ClaimNeighborhoodObjectRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ClaimNeighborhoodObjectRewardCommand.Decode(stream, environment)
        ),
        [ShopPurchaseResultServerCommandType] = new(
            typeof(ShopPurchaseResultServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ShopPurchaseResultServerCommand.Decode(stream, environment)
        ),
    };
}
