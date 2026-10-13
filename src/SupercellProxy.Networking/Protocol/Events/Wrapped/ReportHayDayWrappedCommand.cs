using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Wrapped;

/// <summary>Reports the Hay Day Wrapped page state.</summary>
/// <param name="Locale">The page locale.</param>
/// <param name="State">The page state.</param>
public sealed record ReportHayDayWrappedCommand(string? Locale, int State) : CodecCommand<ReportHayDayWrappedCommand>, ICommandCodecProvider<ReportHayDayWrappedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ReportHayDayWrappedCommand> Codec { get; } = Codecs.Command(
        type: 679,
        Codecs.Composite(
            Codecs.OptionalString,
            static command => command.Locale,
            Codecs.VarInt,
            static command => command.State,
            static (locale, state) => new ReportHayDayWrappedCommand(locale, state)
        ),
        CommandLayout.MetadataFirst
    );
}
