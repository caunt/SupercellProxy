using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Designs;

/// <summary>Publishes or withdraws a decoration event design.</summary>
/// <param name="Published">True to make the design visible to other players; false to take it back.</param>
/// <param name="DesignIndex">Which of the player's saved designs it concerns.</param>
public sealed record PublishDecorationEventDesignCommand(bool Published, int DesignIndex) : CodecCommand<PublishDecorationEventDesignCommand>, ICommandCodecProvider<PublishDecorationEventDesignCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PublishDecorationEventDesignCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Boolean,
            static command => command.Published,
            Codecs.VarInt,
            static command => command.DesignIndex,
            static (published, designIndex) => new PublishDecorationEventDesignCommand(published, designIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
