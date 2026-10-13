using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Decorations;

/// <summary>Delivers a decoration design entry to the design manager.</summary>
/// <param name="Name">The name.</param>
/// <param name="Design">The design.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Text">The text.</param>
/// <param name="Value2">The value2.</param>
public sealed record DecorationDesignMessage(string? Name, DecorationDesign? Design, int Value0, int Value1, string? Text, int Value2) : CodecMessage<DecorationDesignMessage>, ICodecProvider<DecorationDesignMessage>
{
    /// <inheritdoc />
    public static ICodec<DecorationDesignMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Name,
        Codecs.Optional(DecorationDesign.Codec),
        static message => message.Design,
        Codecs.VarInt,
        static message => message.Value0,
        Codecs.VarInt,
        static message => message.Value1,
        Codecs.OptionalString,
        static message => message.Text,
        Codecs.VarInt,
        static message => message.Value2,
        static (name, design, value0, value1, text, value2) => new DecorationDesignMessage(name, design, value0, value1, text, value2)
    );
}
