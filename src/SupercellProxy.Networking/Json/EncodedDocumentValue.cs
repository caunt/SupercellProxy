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

    /// <summary>Encodes a value with its concrete contract, as native writes it.</summary>
    /// <typeparam name="TValue">The contract whose System.Text.Json shape is written; the static type decides which members are serialized.</typeparam>
    /// <param name="value">The value to serialize; <see langword="null"/> is stored as a JSON <see langword="null"/> literal, which <see cref="Decode{TValue}"/> later refuses.</param>
    /// <returns>A new value holding the UTF-8 JSON of <paramref name="value"/>, written back verbatim when the enclosing document is serialized.</returns>
    public static EncodedDocumentValue Create<TValue>(TValue value)
    {
        return new EncodedDocumentValue(JsonSerializer.SerializeToUtf8Bytes(value));
    }

    /// <summary>Deserializes the preserved value through a concrete contract.</summary>
    /// <typeparam name="TValue">The contract to read the retained JSON into, chosen by the caller once the field's shape is known.</typeparam>
    /// <returns>A new <typeparamref name="TValue"/> read from the retained bytes; each call deserializes again, so changes to it are not kept.</returns>
    /// <exception cref="InvalidDataException">The retained JSON is a bare <see langword="null"/> literal.</exception>
    public TValue Decode<TValue>()
    {
        return JsonSerializer.Deserialize<TValue>(_encodedValue)
            ?? throw new InvalidDataException(message: "The encoded JSON value is null.");
    }

    /// <summary>Returns the retained UTF-8 JSON bytes without exposing mutable codec storage.</summary>
    /// <returns>A fresh copy of the JSON bytes exactly as they were read or created, so changing it does not change this value.</returns>
    public byte[] Encode()
    {
        return [.. _encodedValue];
    }
}
