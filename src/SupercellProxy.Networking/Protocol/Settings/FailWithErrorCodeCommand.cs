using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Always fails; native stores the field as the execution error code.</summary>
/// <param name="ErrorCode">The error code the command fails with.</param>
public sealed record FailWithErrorCodeCommand(int ErrorCode) : CodecCommand<FailWithErrorCodeCommand>, ICommandCodecProvider<FailWithErrorCodeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FailWithErrorCodeCommand> Codec { get; } = Codecs.Command(
        type: 373,
        Codecs.Composite(Codecs.VarInt, static command => command.ErrorCode, static (errorCode) => new FailWithErrorCodeCommand(errorCode))
    );
}
