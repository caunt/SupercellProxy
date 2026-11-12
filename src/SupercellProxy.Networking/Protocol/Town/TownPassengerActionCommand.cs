using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Applies a passenger action such as booking movement or holding the visitor for interaction.</summary>
/// <param name="PassengerGlobalId">Game object id of the passenger in the player's own town.</param>
/// <param name="ActionCode">Action to apply, one of the <c>*Action</c> constants on this type (1 to 5), written as a VarInt after the passenger id.</param>
public sealed record TownPassengerActionCommand(int PassengerGlobalId, int ActionCode) : CodecCommand<TownPassengerActionCommand>, ICommandCodecProvider<TownPassengerActionCommand>
{
    /// <summary>Completes a passenger's personal-train pickup.</summary>
    public const int CompletePersonalTrainPickupAction = 5;

    /// <summary>Moves the passenger toward a booked service building.</summary>
    public const int GoToServiceBuildingAction = 1;

    /// <summary>Sends the passenger toward the train.</summary>
    public const int GoToTrainAction = 2;

    /// <summary>Holds the passenger in place during an interaction.</summary>
    public const int HoldForInteractionAction = 3;

    /// <summary>Releases the passenger after an interaction.</summary>
    public const int ReleaseInteractionAction = 4;

    /// <inheritdoc />
    public static CommandCodec<TownPassengerActionCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.PassengerGlobalId,
        Codecs.VarInt,
        static command => command.ActionCode,
        static (passenger, action) => new TownPassengerActionCommand(passenger, action)));
}
