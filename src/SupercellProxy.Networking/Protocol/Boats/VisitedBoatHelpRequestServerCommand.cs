using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Clientbound notice that the owner of the farm being visited asked for help with a boat crate, so the visitor can fill it. The
/// visitor's copy gains an open help request for that crate unless one is already there.
/// </summary>
/// <param name="HomeOwnerId">Home id of the farm that asked for help.</param>
/// <param name="CrateIndex">Position of the crate in the boat order.</param>
/// <param name="RequestKind">Who the request is addressed to: 1 friends, 2 the neighbourhood, 3 through the Farm Pass perk.</param>
public sealed record VisitedBoatHelpRequestServerCommand([property: JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId, int CrateIndex, int RequestKind) : CodecServerCommand<VisitedBoatHelpRequestServerCommand>, ICommandCodecProvider<VisitedBoatHelpRequestServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<VisitedBoatHelpRequestServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.HomeOwnerId,
            Codecs.VarInt,
            static command => command.CrateIndex,
            Codecs.VarInt,
            static command => command.RequestKind,
            static (owner, crate, requestKind) => new VisitedBoatHelpRequestServerCommand(owner, crate, requestKind)
        )
    );
}
