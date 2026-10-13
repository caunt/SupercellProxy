using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Request Newspaper Command contract.
/// </summary>
/// <summary>
/// Defines the Spend Diamonds contract.
/// </summary>
/// <summary>
/// Defines the Country Code contract.
/// </summary>
/// <summary>
/// Defines the Alternate contract.
/// </summary>
public sealed record RequestNewspaperCommand(bool SpendDiamonds, string CountryCode, bool Alternate)
    : CodecCommand<RequestNewspaperCommand>, ICommandCodecProvider<RequestNewspaperCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestNewspaperCommand> Codec { get; } = Codecs.Command(
        type: 661,
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
