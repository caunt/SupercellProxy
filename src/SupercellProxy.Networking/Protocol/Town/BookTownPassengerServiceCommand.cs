using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Books a passenger into a town service slot.</summary>
public sealed record BookTownPassengerServiceCommand(int PassengerGlobalId, int ServiceIndex, int TutorialDataGlobalId) : CodecCommand<BookTownPassengerServiceCommand>, ICommandCodecProvider<BookTownPassengerServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BookTownPassengerServiceCommand> Codec { get; } = Codecs.Command(
        type: 145,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PassengerGlobalId,
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.TutorialDataGlobalId,
            static (passenger, service, tutorial) => new BookTownPassengerServiceCommand(passenger, service, tutorial)
        )
    );
}
