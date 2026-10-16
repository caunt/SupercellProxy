using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Wrapped;

/// <summary>Opens the Hay Day Wrapped page.</summary>
/// <param name="Link">Address of the player's year-in-review page that was opened.</param>
public sealed record OpenHayDayWrappedCommand(string? Link) : CodecCommand<OpenHayDayWrappedCommand>, ICommandCodecProvider<OpenHayDayWrappedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenHayDayWrappedCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.OptionalString, static command => command.Link, static link => new OpenHayDayWrappedCommand(link)));
}
