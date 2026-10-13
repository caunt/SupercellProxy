using System.Globalization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Canvas;

/// <summary>Saves the avatar's decoration event canvas design.</summary>
/// <param name="EventId">The decoration event id.</param>
/// <param name="EventVariantId">The decoration event variant id.</param>
/// <param name="Design">The compressed canvas design.</param>
/// <param name="DesignId">The id of the design being replaced, when there is one.</param>
public sealed record SaveDecorationEventCanvasCommand(int EventId, int EventVariantId, ReadOnlyMemory<byte> Design, LongId? DesignId) : CodecCommand<SaveDecorationEventCanvasCommand>, ICommandCodecProvider<SaveDecorationEventCanvasCommand>
{
    /// <inheritdoc />
    /// <remarks>Native writes the design length before the optional design id and the design bytes after it.</remarks>
    public static CommandCodec<SaveDecorationEventCanvasCommand> Codec { get; } = Codecs.Command(
        Codecs.Create(
            static stream =>
            {
                int eventId = stream.ReadInt32();
                int eventVariantId = stream.ReadInt32();
                int designLength = stream.ReadInt32();
                LongId? designId = stream.ReadOptionalLongId();

                return designLength < 0 || designLength > stream.Length - stream.Position
                    ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid decoration canvas design length: {designLength}."))
                    : new SaveDecorationEventCanvasCommand(eventId, eventVariantId, stream.ReadBytes(designLength), designId);
            },
            static (stream, command) =>
            {
                stream.WriteInt32(command.EventId);
                stream.WriteInt32(command.EventVariantId);
                stream.WriteInt32(command.Design.Length);
                stream.WriteOptionalLongId(command.DesignId);
                stream.Write(command.Design.Span);
            }
        ),
        CommandLayout.MetadataFirst
    );
}
