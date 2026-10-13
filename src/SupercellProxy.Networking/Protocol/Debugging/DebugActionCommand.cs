using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Debugging;

/// <summary>Development-only command that runs a numbered debug action.</summary>
/// <param name="Action">The action.</param>
/// <param name="AdditionalData">The additional data.</param>
/// <param name="TargetGlobalId">The target data id.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Text">The text.</param>
public sealed record DebugActionCommand(int Action, int AdditionalData, int TargetGlobalId, int Amount, string? Text) : CodecCommand<DebugActionCommand>, ICommandCodecProvider<DebugActionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DebugActionCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Action,
            Codecs.VarInt,
            static command => command.AdditionalData,
            Codecs.VarInt,
            static command => command.TargetGlobalId,
            Codecs.VarInt,
            static command => command.Amount,
            Codecs.OptionalString,
            static command => command.Text,
            static (action, additionalData, targetGlobalId, amount, text) => new DebugActionCommand(action, additionalData, targetGlobalId, amount, text)
        ),
        CommandLayout.FieldsFirst,
        productionAllowed: false
    );
}
