using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Names;

/// <summary>Sets the player's name when it is short enough.</summary>
/// <param name="Name">New player name; native ignores it when it is longer than allowed.</param>
public sealed record SetPlayerNameCommand(string? Name) : CodecCommand<SetPlayerNameCommand>, ICommandCodecProvider<SetPlayerNameCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetPlayerNameCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.OptionalString, static command => command.Name, static name => new SetPlayerNameCommand(name)),
        CommandLayout.MetadataFirst
    );
}
