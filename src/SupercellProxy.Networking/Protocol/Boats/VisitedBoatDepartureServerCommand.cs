using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat Departure Server Command contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
public sealed record VisitedBoatDepartureServerCommand([property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId) : CodecServerCommand<VisitedBoatDepartureServerCommand>, ICommandCodecProvider<VisitedBoatDepartureServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<VisitedBoatDepartureServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.LongId, static command => command.HomeOwnerId, static (owner) => new VisitedBoatDepartureServerCommand(owner))
    );
}
