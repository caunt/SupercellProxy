using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>Grants resources to the avatar.</summary>
/// <param name="Resources">The resources.</param>
public sealed record GrantResourcesServerCommand(ReadOnlyMemory<GrantedResource> Resources) : CodecServerCommand<GrantResourcesServerCommand>, ICommandCodecProvider<GrantResourcesServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<GrantResourcesServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Memory(GrantedResource.Codec),
            static command => command.Resources,
            static (resources) => new GrantResourcesServerCommand(resources)
        )
    );
}
