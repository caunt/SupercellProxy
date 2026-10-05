using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Books a passenger into a town service slot.</summary>
public sealed record BookTownPassengerServiceCommand(int PassengerGlobalId, int ServiceIndex, int TutorialDataGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.BookTownPassengerServiceCommandType;

    /// <summary>Decodes the passenger, service, and optional tutorial.</summary>
    public static BookTownPassengerServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int passenger = stream.ReadVarInt();
        int service = stream.ReadVarInt();
        int tutorial = stream.ReadVarInt();

        return new BookTownPassengerServiceCommand(passenger, service, tutorial);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PassengerGlobalId);
        stream.WriteVarInt(ServiceIndex);
        stream.WriteVarInt(TutorialDataGlobalId);
    }
}
