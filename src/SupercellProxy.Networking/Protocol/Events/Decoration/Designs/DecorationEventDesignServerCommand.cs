using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Designs;

/// <summary>Updates a decoration event design.</summary>
/// <param name="Points">Score the design has earned in the event so far.</param>
/// <param name="EventId">Decoration event the design was submitted to.</param>
/// <param name="DesignId">Server identifier of the submitted design.</param>
/// <param name="VariantId">Variant of that event the player is in.</param>
/// <param name="DesignName">Name the player gave the design.</param>
/// <param name="Votes">Votes other players have cast for the design.</param>
public sealed record DecorationEventDesignServerCommand(int Points, int EventId, long DesignId, int VariantId, string? DesignName, int Votes) : CodecServerCommand<DecorationEventDesignServerCommand>, ICommandCodecProvider<DecorationEventDesignServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DecorationEventDesignServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Points,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarLong,
            static command => command.DesignId,
            Codecs.VarInt,
            static command => command.VariantId,
            Codecs.OptionalString,
            static command => command.DesignName,
            Codecs.VarInt,
            static command => command.Votes,
            static (points, eventId, designId, variantId, designName, votes) => new DecorationEventDesignServerCommand(points, eventId, designId, variantId, designName, votes)
        ),
        CommandLayout.MetadataFirst
    );
}
