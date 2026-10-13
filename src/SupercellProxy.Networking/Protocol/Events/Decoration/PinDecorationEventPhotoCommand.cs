using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Pins or unpins a photo in the decoration event album.</summary>
/// <param name="Pinned">Whether the photo is pinned.</param>
/// <param name="PhotoIndex">The photo slot.</param>
public sealed record PinDecorationEventPhotoCommand(bool Pinned, int PhotoIndex) : CodecCommand<PinDecorationEventPhotoCommand>, ICommandCodecProvider<PinDecorationEventPhotoCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PinDecorationEventPhotoCommand> Codec { get; } = Codecs.Command(
        type: 625,
        Codecs.Composite(
            Codecs.Boolean,
            static command => command.Pinned,
            Codecs.VarInt,
            static command => command.PhotoIndex,
            static (pinned, photoIndex) => new PinDecorationEventPhotoCommand(pinned, photoIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
