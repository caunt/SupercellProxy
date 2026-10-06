using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// A command whose native class adds no fields to <see cref="Command"/>.
/// </summary>
public sealed record CommandWithNoFields : Command
{
    /// <summary>
    /// Provides the Command Types value or operation.
    /// </summary>
    public static readonly int[] CommandTypes =
    [
        84,
        97,
        391,
        503,
        505,
        507,
        508,
        CommandRegistry.CollectMovieTicketRewardCommandType,
        513,
        515,
        CommandRegistry.ResetWheelCarCommandType,
        524,
        CommandRegistry.MarkMapGameCompletedQuestsSeenCommandType,
        527,
        CommandRegistry.ClientCommand528Type,
        529,
        CommandRegistry.AcknowledgePerformanceProfilingCommandType,
        533,
        536,
        CommandRegistry.EnterBoyIntervalRestCommandType,
        CommandRegistry.MarkFarmPassTasksSeenCommandType,
        541,
        546,
        547,
        548,
        551,
        553,
        554,
        CommandRegistry.PurchaseRoadsideAdvertisementCreditCommandType,
        557,
        562,
        CommandRegistry.ClearMovieTicketShopNotificationsCommandType,
        CommandRegistry.CollectSanctuaryVisitorGiftCommandType,
        571,
        572,
        575,
        CommandRegistry.MarkMapGameCurrentQuestsSeenCommandType,
        580,
        582,
        CommandRegistry.RejectBoyOfferCommandType,
        CommandRegistry.StartWheelSpinCommandType,
        CommandRegistry.CloseWheelCarCommandType,
        CommandRegistry.MarkMapGameSunPointsSeenCommandType,
        593,
        CommandRegistry.WakeBoyFromRestCommandType,
        CommandRegistry.MarkNeighborhoodTasksSeenCommandType,
        598,
        604,
        613,
        CommandRegistry.ClaimMapGameFuelPrizeCommandType,
        615,
        CommandRegistry.SpinMapGameFuelWheelCommandType,
        622,
        628,
        630,
        CommandRegistry.UnlockRoadsideStandCommandType,
        633,
        635,
        CommandRegistry.BaseEventSceneCommandType,
        639,
        640,
        CommandRegistry.StartFarmPassSeasonCommandType,
        645,
        647,
        652,
        CommandRegistry.CompleteBoyInteractionCommandType,
        659,
        CommandRegistry.CollectHelperAreaCommandType,
        CommandRegistry.CompleteBoatOrderCommandType,
        CommandRegistry.AdvanceBoatStateCommandType,
        664,
        668,
        671,
        CommandRegistry.AcknowledgeBoatCommandType,
        676,
        678,
        681,
        682,
        683,
        CommandRegistry.AdvanceReengagementFlowCommandType,
        685,
        688,
        695,
        697,
        698,
        699,
    ];

    /// <summary>
    /// Initializes a new <see cref="CommandWithNoFields"/> instance.
    /// </summary>
    public CommandWithNoFields(int type)
    {
        Type = type;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandWithNoFields Decode(int type, MessageStream stream, CommandEnvironment environment)
    {
        return new CommandWithNoFields(type);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
    }
}
