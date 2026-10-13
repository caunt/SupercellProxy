using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Reports a shop purchase request outcome without granting the purchased package again.</summary>
public sealed record ShopPurchaseResultServerCommand(int EventId, int Variant, int PackageGlobalId, int Index, bool Succeeded, int ErrorCode, string? ErrorText) : CodecServerCommand<ShopPurchaseResultServerCommand>, ICommandCodecProvider<ShopPurchaseResultServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ShopPurchaseResultServerCommand> Codec { get; } = Codecs.Command(
        type: 228,
        Codecs.Composite(
            Codecs.Int32,
            static command => command.EventId,
            Codecs.Int32,
            static command => command.Variant,
            Codecs.Int32,
            static command => command.PackageGlobalId,
            Codecs.VarInt,
            static command => command.Index,
            Outcome.Codec,
            static command => new Outcome(command.Succeeded, command.ErrorCode, command.ErrorText),
            static (eventId, variant, packageGlobalId, index, outcome) => new ShopPurchaseResultServerCommand(eventId, variant, packageGlobalId, index, outcome.Succeeded, outcome.ErrorCode, outcome.ErrorText)
        )
    );

    // Native reads the error code and text only for a failed purchase.
    private readonly record struct Outcome(bool Succeeded, int ErrorCode, string? ErrorText)
    {
        public static ICodec<Outcome> Codec { get; } = Codecs.Dispatch(
            Codecs.Boolean,
            static outcome => outcome.Succeeded,
            [
                new KeyValuePair<bool, ICodec<Outcome>>(key: true, Codecs.Unit(static () => new Outcome(Succeeded: true, ErrorCode: 0, ErrorText: null))),
                new KeyValuePair<bool, ICodec<Outcome>>(
                    key: false,
                    Codecs.Composite(
                        Codecs.Int32,
                        static outcome => outcome.ErrorCode,
                        Codecs.String,
                        static outcome => outcome.ErrorText ?? string.Empty,
                        static (errorCode, errorText) => new Outcome(Succeeded: false, errorCode, errorText)
                    )
                ),
            ]
        );
    }
}
