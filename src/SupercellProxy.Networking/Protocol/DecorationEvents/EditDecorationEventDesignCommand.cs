using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DecorationEvents;

/// <summary>Reopens the submitted decoration event design for editing.</summary>
public sealed record EditDecorationEventDesignCommand : CodecCommand<EditDecorationEventDesignCommand>, ICommandCodecProvider<EditDecorationEventDesignCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EditDecorationEventDesignCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new EditDecorationEventDesignCommand()));
}
