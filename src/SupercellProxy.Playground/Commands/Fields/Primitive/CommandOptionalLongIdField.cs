using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandOptionalLongIdField</c>.
/// </summary>
public sealed record CommandOptionalLongIdField(LongId? Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.OptionalLongId;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);

        if (Value is not null)
            stream.WriteLongId(Value.Value);
    }
}
