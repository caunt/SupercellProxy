using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Deeplinks;

/// <summary>Opens a deeplink for the avatar.</summary>
/// <param name="HomeId">The home id.</param>
/// <param name="DeeplinkGlobalId">The deeplink data id.</param>
/// <param name="Parameters">The parameters.</param>
public sealed record OpenDeeplinkServerCommand(LongId HomeId, int DeeplinkGlobalId, ReadOnlyMemory<string?> Parameters) : CodecServerCommand<OpenDeeplinkServerCommand>, ICommandCodecProvider<OpenDeeplinkServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenDeeplinkServerCommand> Codec { get; } = Codecs.Command(
        type: 266,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.HomeId,
            Codecs.VarInt,
            static command => command.DeeplinkGlobalId,
            Codecs.Memory(Codecs.OptionalString),
            static command => command.Parameters,
            static (homeId, deeplinkGlobalId, parameters) => new OpenDeeplinkServerCommand(homeId, deeplinkGlobalId, parameters)
        )
    );
}
