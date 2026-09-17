using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents a map-game participant emblem.</summary>
public sealed record MapGamePawnEmblem(
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown0")] int Background,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown1")] int Pattern,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown2")] int Symbol
)
{
    /// <summary>Decodes an emblem from the supplied protocol payload.</summary>
    public static MapGamePawnEmblem Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGamePawnEmblem(stream.ReadVariableInt(), stream.ReadVariableInt(), stream.ReadVariableInt());
    }

    /// <summary>Encodes this emblem into the supplied protocol payload.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Background);
        stream.WriteVariableInt(Pattern);
        stream.WriteVariableInt(Symbol);
    }
}
