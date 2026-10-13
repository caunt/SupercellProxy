using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Stores the app rating prompt state.</summary>
/// <param name="State">The new app rating prompt state.</param>
public sealed record SetAppRateStateCommand(int State) : CodecCommand<SetAppRateStateCommand>, ICommandCodecProvider<SetAppRateStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetAppRateStateCommand> Codec { get; } = Codecs.Command(type: 49, Codecs.Composite(Codecs.VarInt, static command => command.State, static (state) => new SetAppRateStateCommand(state)));
}
