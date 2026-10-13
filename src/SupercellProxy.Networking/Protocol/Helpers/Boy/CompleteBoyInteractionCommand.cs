using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Completes the interaction with the arrived boy.</summary>
public sealed record CompleteBoyInteractionCommand : CodecCommand<CompleteBoyInteractionCommand>, ICommandCodecProvider<CompleteBoyInteractionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteBoyInteractionCommand> Codec { get; } = Codecs.Command(type: 653, Codecs.Unit(static () => new CompleteBoyInteractionCommand()));
}
