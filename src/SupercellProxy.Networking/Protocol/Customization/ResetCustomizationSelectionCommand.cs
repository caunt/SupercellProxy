using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Clears the current customization selection.</summary>
public sealed record ResetCustomizationSelectionCommand : CodecCommand<ResetCustomizationSelectionCommand>, ICommandCodecProvider<ResetCustomizationSelectionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ResetCustomizationSelectionCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ResetCustomizationSelectionCommand()));
}
