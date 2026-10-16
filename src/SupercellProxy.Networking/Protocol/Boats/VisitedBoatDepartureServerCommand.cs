using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Clientbound notice that the boat of the farm being visited has sailed off: the visitor's copy marks it departed and drops its crate
/// help requests. Ignored unless the visitor is on that owner's farm.
/// </summary>
/// <param name="HomeOwnerId">Home id of the farm whose boat departed.</param>
public sealed record VisitedBoatDepartureServerCommand([property: JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId) : CodecServerCommand<VisitedBoatDepartureServerCommand>, ICommandCodecProvider<VisitedBoatDepartureServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<VisitedBoatDepartureServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.LongId, static command => command.HomeOwnerId, static owner => new VisitedBoatDepartureServerCommand(owner))
    );
}
