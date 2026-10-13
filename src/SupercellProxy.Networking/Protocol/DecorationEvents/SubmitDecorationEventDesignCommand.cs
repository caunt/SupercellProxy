using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DecorationEvents;

/// <summary>Submits the decoration event design.</summary>
public sealed record SubmitDecorationEventDesignCommand : CodecCommand<SubmitDecorationEventDesignCommand>, ICommandCodecProvider<SubmitDecorationEventDesignCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SubmitDecorationEventDesignCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new SubmitDecorationEventDesignCommand()));
}
