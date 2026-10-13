using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat Help Request Server Command contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Crate Index contract.
/// </summary>
/// <summary>
/// Defines the Request Value contract.
/// </summary>
public sealed record VisitedBoatHelpRequestServerCommand([property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId, int CrateIndex, int RequestKind) : CodecServerCommand<VisitedBoatHelpRequestServerCommand>, ICommandCodecProvider<VisitedBoatHelpRequestServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<VisitedBoatHelpRequestServerCommand> Codec { get; } = Codecs.Command(
        type: 388,
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
