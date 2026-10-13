using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Progression;

/// <summary>Acknowledges the level-up screen for the given experience level.</summary>
/// <param name="Level">The level.</param>
public sealed record AcknowledgeLevelUpCommand(ushort Level) : CodecCommand<AcknowledgeLevelUpCommand>, ICommandCodecProvider<AcknowledgeLevelUpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgeLevelUpCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.UInt16, static command => command.Level, static (level) => new AcknowledgeLevelUpCommand(level)));
}
