using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Json;

/// <summary>Retains an unknown JSON value inside the codec until a concrete contract is available.</summary>
[JsonConverter(typeof(EncodedDocumentValueConverter))]
public sealed class EncodedDocumentValue
{
    private readonly byte[] _encodedValue;

    internal EncodedDocumentValue(byte[] encodedValue)
    {
        _encodedValue = encodedValue;
    }

    /// <summary>Deserializes the preserved value through a concrete contract.</summary>
    public TValue Decode<TValue>()
    {
        return JsonSerializer.Deserialize<TValue>(_encodedValue)
            ?? throw new InvalidDataException(message: "The encoded JSON value is null.");
    }

    /// <summary>Returns the retained UTF-8 JSON bytes without exposing mutable codec storage.</summary>
    public byte[] Encode()
    {
        return [.. _encodedValue];
    }
}
