using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Clears the active task from the player's farm-side derby manager.</summary>
public sealed record ClearActiveDerbyTaskCommand : CodecCommand<ClearActiveDerbyTaskCommand>, ICommandCodecProvider<ClearActiveDerbyTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClearActiveDerbyTaskCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClearActiveDerbyTaskCommand()), CommandLayout.MetadataFirst);
}
