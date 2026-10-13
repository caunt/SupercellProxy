using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Designs;

/// <summary>Updates a decoration event design.</summary>
/// <param name="Points">The points.</param>
/// <param name="EventId">The event id.</param>
/// <param name="DesignId">The design id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="DesignName">The design name.</param>
/// <param name="Votes">The votes.</param>
public sealed record DecorationEventDesignServerCommand(int Points, int EventId, long DesignId, int VariantId, string? DesignName, int Votes) : CodecServerCommand<DecorationEventDesignServerCommand>, ICommandCodecProvider<DecorationEventDesignServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DecorationEventDesignServerCommand> Codec { get; } = Codecs.Command(
        type: 771,
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
