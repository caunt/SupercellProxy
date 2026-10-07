using SupercellProxy.Networking.Protocol.Events.Derby.Messages;
using SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;
using SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

public static partial class MessageRegistry
{
    /// <summary>Identifies additional-task purchase requests.</summary>
    public const ushort BuyExtraDerbyTaskMessageType = 12056;
    /// <summary>Identifies additional-task purchase results.</summary>
    public const ushort BuyExtraDerbyTaskResponseMessageType = 24089;
    /// <summary>Identifies the complete derby board response.</summary>
    public const ushort DerbyBoardMessageType = 23074;
    /// <summary>Identifies an individual neighborhood race-entry update.</summary>
    public const ushort DerbyRaceUpdateMessageType = 21436;
    /// <summary>Identifies the derby neighborhood rankings response.</summary>
    public const ushort DerbyRankingsMessageType = 23227;
    /// <summary>Identifies the neighborhood derby task-log response.</summary>
    public const ushort DerbyTaskLogMessageType = 23139;
    /// <summary>Identifies the request to resolve or discard the player's selected task.</summary>
    public const ushort DiscardDerbyTaskMessageType = 19163;
    /// <summary>Identifies task reactivation requests.</summary>
    public const ushort ReactivateDerbyTaskMessageType = 12690;
    /// <summary>Identifies the derby board request.</summary>
    public const ushort RequestDerbyBoardMessageType = 16886;
    /// <summary>Identifies the request for one current or previous derby race.</summary>
    public const ushort RequestDerbyRaceMessageType = 15980;
    /// <summary>Identifies the derby neighborhood rankings request.</summary>
    public const ushort RequestDerbyRankingsMessageType = 15762;
    /// <summary>Identifies the neighborhood derby task-log request.</summary>
    public const ushort RequestDerbyTaskLogMessageType = 13511;
    /// <summary>Identifies paid board-slot refresh requests.</summary>
    public const ushort SpeedUpDerbyBoardTaskMessageType = 19071;
    /// <summary>Identifies paid board-slot refresh results.</summary>
    public const ushort SpeedUpDerbyBoardTaskResponseMessageType = 26106;
    /// <summary>Identifies the request to take a derby task.</summary>
    public const ushort TakeDerbyTaskMessageType = 19110;
    /// <summary>Identifies removal of an available board task.</summary>
    public const ushort TrashDerbyBoardTaskMessageType = 18447;

    private static Dictionary<ushort, MessageRegistryEntry> RegisterDerbyMessages(Dictionary<ushort, MessageRegistryEntry> registrations)
    {
        registrations[RequestDerbyBoardMessageType] = new(typeof(RequestDerbyBoardMessage), RequestDerbyBoardMessage.Decode) { CaptureName = nameof(RequestDerbyBoardMessage) };
        registrations[RequestDerbyTaskLogMessageType] = new(typeof(RequestDerbyTaskLogMessage), RequestDerbyTaskLogMessage.Decode) { CaptureName = nameof(RequestDerbyTaskLogMessage) };
        registrations[DerbyTaskLogMessageType] = new(typeof(DerbyTaskLogMessage), DerbyTaskLogMessage.Decode) { CaptureName = nameof(DerbyTaskLogMessage) };
        registrations[RequestDerbyRankingsMessageType] = new(typeof(RequestDerbyRankingsMessage), RequestDerbyRankingsMessage.Decode) { CaptureName = nameof(RequestDerbyRankingsMessage) };
        registrations[RequestDerbyRaceMessageType] = new(typeof(RequestDerbyRaceMessage), RequestDerbyRaceMessage.Decode) { CaptureName = nameof(RequestDerbyRaceMessage) };
        registrations[DerbyRankingsMessageType] = new(typeof(DerbyRankingsMessage), DerbyRankingsMessage.Decode) { CaptureName = nameof(DerbyRankingsMessage) };
        registrations[DerbyRaceUpdateMessageType] = new(typeof(DerbyRaceUpdateMessage), DerbyRaceUpdateMessage.Decode) { CaptureName = nameof(DerbyRaceUpdateMessage) };
        registrations[DerbyBoardMessageType] = new(typeof(DerbyBoardMessage), DerbyBoardMessage.Decode) { CaptureName = "Clientbound23074Message" };
        registrations[TakeDerbyTaskMessageType] = new(typeof(TakeDerbyTaskMessage), TakeDerbyTaskMessage.Decode) { CaptureName = nameof(TakeDerbyTaskMessage) };
        registrations[DiscardDerbyTaskMessageType] = new(typeof(DiscardDerbyTaskMessage), DiscardDerbyTaskMessage.Decode) { CaptureName = nameof(DiscardDerbyTaskMessage) };
        registrations[ReactivateDerbyTaskMessageType] = new(typeof(ReactivateDerbyTaskMessage), ReactivateDerbyTaskMessage.Decode) { CaptureName = nameof(ReactivateDerbyTaskMessage) };
        registrations[TrashDerbyBoardTaskMessageType] = new(typeof(TrashDerbyBoardTaskMessage), TrashDerbyBoardTaskMessage.Decode) { CaptureName = nameof(TrashDerbyBoardTaskMessage) };
        registrations[SpeedUpDerbyBoardTaskMessageType] = new(typeof(SpeedUpDerbyBoardTaskMessage), SpeedUpDerbyBoardTaskMessage.Decode) { CaptureName = nameof(SpeedUpDerbyBoardTaskMessage) };
        registrations[SpeedUpDerbyBoardTaskResponseMessageType] = new(typeof(SpeedUpDerbyBoardTaskResponseMessage), SpeedUpDerbyBoardTaskResponseMessage.Decode) { CaptureName = nameof(SpeedUpDerbyBoardTaskResponseMessage) };
        registrations[BuyExtraDerbyTaskMessageType] = new(typeof(BuyExtraDerbyTaskMessage), BuyExtraDerbyTaskMessage.Decode) { CaptureName = nameof(BuyExtraDerbyTaskMessage) };
        registrations[BuyExtraDerbyTaskResponseMessageType] = new(typeof(BuyExtraDerbyTaskResponseMessage), BuyExtraDerbyTaskResponseMessage.Decode) { CaptureName = nameof(BuyExtraDerbyTaskResponseMessage) };

        return registrations;
    }
}
