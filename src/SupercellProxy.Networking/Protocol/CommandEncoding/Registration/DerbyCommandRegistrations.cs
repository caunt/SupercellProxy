using SupercellProxy.Networking.Protocol.Events.Derby.Commands;
using SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;
using SupercellProxy.Networking.Protocol.Inventory.Currency;
using SupercellProxy.Networking.Protocol.Neighborhoods.Membership;
using SupercellProxy.Networking.Protocol.MessageEncoding;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class DerbyCommandRegistrations
{
    internal static readonly Dictionary<int, CommandRegistryEntry> Entries = new()
    {
        [AdjustDiamondsServerCommandType] = new(
            typeof(AdjustDiamondsServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => AdjustDiamondsServerCommand.Decode(stream, environment)
        ),
        [SpendDiamondsServerCommandType] = new(
            typeof(SpendDiamondsServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => SpendDiamondsServerCommand.Decode(stream, environment)
        ),
        [SetCurrentDerbyLeagueServerCommandType] = new(
            typeof(SetCurrentDerbyLeagueServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => SetCurrentDerbyLeagueServerCommand.Decode(stream, environment)
        ),
        [RequestAutomaticNeighborhoodCreationCommandType] = new(
            typeof(RequestAutomaticNeighborhoodCreationCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => RequestAutomaticNeighborhoodCreationCommand.Decode(stream, environment)
        ),
        [JoinNeighborhoodServerCommandType] = new(
            typeof(JoinNeighborhoodServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => JoinNeighborhoodServerCommand.Decode(stream, environment)
        ),
        [RecordDerbyHelpServerCommandType] = new(
            typeof(RecordDerbyHelpServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => RecordDerbyHelpServerCommand.Decode(stream, environment)
        ),
        [SetDerbyTaskTimeServerCommandType] = new(
            typeof(SetDerbyTaskTimeServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => SetDerbyTaskTimeServerCommand.Decode(stream, environment)
        ),
        [SynchronizeDerbyTaskServerCommandType] = new(
            typeof(SynchronizeDerbyTaskServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => SynchronizeDerbyTaskServerCommand.Decode(stream, environment)
        ),
        [AcknowledgeDerbyEntriesCommandType] = new(
            typeof(AcknowledgeDerbyEntriesCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => AcknowledgeDerbyEntriesCommand.Decode(stream, environment)
        ),
        [ClaimDerbyPlacementRewardsCommandType] = new(
            typeof(ClaimDerbyPlacementRewardsCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ClaimDerbyPlacementRewardsCommand.Decode(stream, environment)
        ),
        [ResetDerbyRewardsCommandType] = new(
            typeof(ResetDerbyRewardsCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ResetDerbyRewardsCommand.Decode(stream, environment)
        ),
        [AssignDerbyServerCommandType] = new(
            typeof(AssignDerbyServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => AssignDerbyServerCommand.Decode(stream, environment)
        ),
        [EndDerbyServerCommandType] = new(
            typeof(EndDerbyServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => EndDerbyServerCommand.Decode(stream, environment)
        ),
        [UpdateDerbyPointsServerCommandType] = new(
            typeof(UpdateDerbyPointsServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => UpdateDerbyPointsServerCommand.Decode(stream, environment)
        ),
        [SetDerbyParticipationServerCommandType] = new(
            typeof(SetDerbyParticipationServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => SetDerbyParticipationServerCommand.Decode(stream, environment)
        ),
        [DerbyNotificationServerCommandType] = new(
            typeof(DerbyNotificationServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => DerbyNotificationServerCommand.Decode(stream, environment)
        ),
        [DerbySeasonNotificationServerCommandType] = new(
            typeof(DerbySeasonNotificationServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => DerbySeasonNotificationServerCommand.Decode(stream, environment)
        ),
        [LeaveNeighborhoodServerCommandType] = new(
            typeof(LeaveNeighborhoodServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => LeaveNeighborhoodServerCommand.Decode(stream, environment)
        ),
        [ExpireDerbyTaskServerCommandType] = new(
            typeof(ExpireDerbyTaskServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ExpireDerbyTaskServerCommand.Decode(stream, environment)
        ),
        [ClearActiveDerbyTaskCommandType] = new(
            typeof(ClearActiveDerbyTaskCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ClearActiveDerbyTaskCommand.Decode(stream, environment)
        ),
        [ResolveDerbyTaskServerCommandType] = new(
            typeof(ResolveDerbyTaskServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ResolveDerbyTaskServerCommand.Decode(stream, environment)
        ),
        [RerollDerbyRewardsCommandType] = new(
            typeof(RerollDerbyRewardsCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => RerollDerbyRewardsCommand.Decode(stream, environment)
        ),
        [ClaimDerbyRewardsCommandType] = new(
            typeof(ClaimDerbyRewardsCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ClaimDerbyRewardsCommand.Decode(stream, environment)
        ),
        [MarkDerbyRewardsSeenCommandType] = new(
            typeof(MarkDerbyRewardsSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => MarkDerbyRewardsSeenCommand.Decode(stream, environment)
        ),
        [StartDerbyTaskServerCommandType] = new(
            typeof(StartDerbyTaskServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => StartDerbyTaskServerCommand.Decode(stream, environment)
        ),
        [ResolveCompletedDerbyTaskCommandType] = new(
            typeof(ResolveCompletedDerbyTaskCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => ResolveCompletedDerbyTaskCommand.Decode(stream, environment)
        ),
        [MarkFlyingDerbyBunniesSeenCommandType] = new(
            typeof(MarkFlyingDerbyBunniesSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedResolver) => MarkFlyingDerbyBunniesSeenCommand.Decode(stream, environment)
        ),
    };
}
