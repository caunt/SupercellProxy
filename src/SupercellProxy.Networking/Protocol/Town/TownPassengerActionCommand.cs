using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Applies a passenger action such as booking movement or holding the visitor for interaction.</summary>
public sealed record TownPassengerActionCommand(int PassengerGlobalId, int ActionCode) : Command
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
    public override int Type => CommandRegistry.TownPassengerActionCommandType;

    /// <summary>Decodes the passenger and action.</summary>
    public static TownPassengerActionCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int passenger = stream.ReadVarInt();
        int action = stream.ReadVarInt();

        return new TownPassengerActionCommand(passenger, action);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PassengerGlobalId);
        stream.WriteVarInt(ActionCode);
    }
}
