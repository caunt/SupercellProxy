using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Wrapped;

/// <summary>Closes the Hay Day Wrapped page and clears its pending state.</summary>
public sealed record CloseHayDayWrappedCommand : CodecCommand<CloseHayDayWrappedCommand>, ICommandCodecProvider<CloseHayDayWrappedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CloseHayDayWrappedCommand> Codec { get; } = Codecs.Command(type: 391, Codecs.Unit(static () => new CloseHayDayWrappedCommand()));
}
