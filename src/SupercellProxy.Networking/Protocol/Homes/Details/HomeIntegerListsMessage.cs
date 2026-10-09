using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes.Details;

/// <summary>Returns two optional, parallel integer lists associated with one home.</summary>
public sealed record HomeIntegerListsMessage(int[]? FirstValues, int[]? SecondValues, LongId? HomeId) : IMessage
{
    /// <summary>Decodes the native list-presence flags, lists, and optional home identifier.</summary>
    public static HomeIntegerListsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int[]? first = stream.ReadBoolean() ? stream.ReadArray(static input => input.ReadVarInt()) : null;
        int[]? second = stream.ReadBoolean() ? stream.ReadArray(static input => input.ReadVarInt()) : null;
        LongId? home = stream.ReadBoolean() ? stream.ReadLongId() : null;

        return stream.Position == stream.Length ? new(first, second, home)
            : throw new InvalidDataException(message: "The home integer lists have trailing data.");
    }

    /// <summary>Preserves absent lists separately from present, empty lists.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(FirstValues is not null);

        if (FirstValues is { } first) stream.WriteArray(first, static (output, value) => output.WriteVarInt(value));

        stream.WriteBoolean(SecondValues is not null);

        if (SecondValues is { } second) stream.WriteArray(second, static (output, value) => output.WriteVarInt(value));

        stream.WriteBoolean(HomeId.HasValue);

        if (HomeId is { } home) stream.WriteLongId(home);
    }

    /// <summary>Omits the home identifier and list contents from routine diagnostics.</summary>
    public override string ToString()
    {
        return nameof(HomeIntegerListsMessage);
    }
}
