using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Applies a passenger action such as booking movement or holding the visitor for interaction.</summary>
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
    public static CommandCodec<TownPassengerActionCommand> Codec { get; } = Codecs.Command(
        type: 140,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PassengerGlobalId,
            Codecs.VarInt,
            static command => command.ActionCode,
            static (passenger, action) => new TownPassengerActionCommand(passenger, action)
        )
    );
}
