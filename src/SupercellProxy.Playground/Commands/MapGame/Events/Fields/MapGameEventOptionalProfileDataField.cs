using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalProfileDataField</c>.
/// </summary>
internal sealed record MapGameEventOptionalProfileDataField(MapGameEventProfileData? Value)
    : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalProfileData;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
