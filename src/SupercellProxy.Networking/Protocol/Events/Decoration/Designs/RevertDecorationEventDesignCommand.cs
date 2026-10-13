using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Designs;

/// <summary>Restores the decoration event design to its last saved state.</summary>
public sealed record RevertDecorationEventDesignCommand : CodecCommand<RevertDecorationEventDesignCommand>, ICommandCodecProvider<RevertDecorationEventDesignCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RevertDecorationEventDesignCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RevertDecorationEventDesignCommand()));
}
