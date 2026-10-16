using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Serverbound command asking for a new newspaper edition; it restarts the newspaper's refresh timer, and the server answers with
/// the edition's roadside shop advertisements.
/// </summary>
/// <param name="SpendDiamonds">Whether diamonds pay for an early refresh before the timer runs out.</param>
/// <param name="CountryCode">Two-letter country code the advertisements are picked for, such as <c>US</c>.</param>
/// <param name="Alternate">Flag the client sends with the request; its effect is not identified.</param>
public sealed record RequestNewspaperCommand(bool SpendDiamonds, string CountryCode, bool Alternate)
    : CodecCommand<RequestNewspaperCommand>, ICommandCodecProvider<RequestNewspaperCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestNewspaperCommand> Codec { get; } = Codecs.Command(
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.String,
                static command => command.CountryCode,
                Codecs.Boolean,
                static command => command.SpendDiamonds,
                Codecs.Boolean,
                static command => command.Alternate,
                static (countryCode, spendDiamonds, alternate) => new RequestNewspaperCommand(spendDiamonds, countryCode, alternate)
            ),
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.SpendDiamonds,
                Codecs.String,
                static command => command.CountryCode,
                Codecs.Boolean,
                static command => command.Alternate,
                static (spendDiamonds, countryCode, alternate) => new RequestNewspaperCommand(spendDiamonds, countryCode, alternate)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
