using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarManagerA</c>.
/// </summary>
public record AvatarManagerA
{
    /// <summary>
    /// Gets or sets the <c>Version</c> value.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Gets or sets the <c>Optional</c> value.
    /// </summary>
    public AvatarManagerAOptional? Optional { get; init; }

    /// <summary>
    /// Gets or sets the <c>FixedValues</c> value.
    /// </summary>
    public KeyValuePair<int, int>[] FixedValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>Pairs</c> value.
    /// </summary>
    public KeyValuePair<int, int>[] Pairs { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>UnknownValues0</c> value.
    /// </summary>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>UnknownValues1</c> value.
    /// </summary>
    public int[] UnknownValues1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>Strings</c> value.
    /// </summary>
    public KeyValuePair<int, string?>[] Strings { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>UnknownEntries0</c> value.
    /// </summary>
    public AvatarManagerAItem[] UnknownEntries0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>UnknownEntries1</c> value.
    /// </summary>
    public AvatarManagerAItem[] UnknownEntries1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>Triples</c> value.
    /// </summary>
    public (int Unknown0, int Unknown1, int Unknown2)[] Triples { get; init; } = [];

    internal static AvatarManagerA Decode(MessageStream stream)
    {
        var version = stream.ReadVarInt();
        var optional = stream.ReadBoolean() ? AvatarManagerAOptional.Decode(stream) : null;

        return new AvatarManagerA
        {
            Version = version,
            Optional = optional,
            FixedValues = stream.ReadArray(static valueStream => new KeyValuePair<int, int>(
                valueStream.ReadVarInt(),
                valueStream.ReadInt32()
            )),
            Pairs = stream.ReadArray(static valueStream => new KeyValuePair<int, int>(
                valueStream.ReadVarInt(),
                valueStream.ReadVarInt()
            )),
            UnknownValues0 = stream.ReadArray(static valueStream => valueStream.ReadVarInt()),
            UnknownValues1 = stream.ReadArray(static valueStream => valueStream.ReadVarInt()),
            Strings = stream.ReadArray(static valueStream => new KeyValuePair<int, string?>(
                valueStream.ReadVarInt(),
                valueStream.ReadOptionalString()
            )),
            UnknownEntries0 = stream.ReadArray(AvatarManagerAItem.Decode),
            UnknownEntries1 = stream.ReadArray(AvatarManagerAItem.Decode),
            Triples = stream.ReadArray(static valueStream =>
                (valueStream.ReadVarInt(), valueStream.ReadVarInt(), valueStream.ReadVarInt())
            ),
        };
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Version);
        stream.WriteBoolean(Optional is not null);
        Optional?.Encode(stream);
        stream.WriteArray(
            FixedValues,
            static (valueStream, value) =>
            {
                valueStream.WriteVarInt(value.Key);
                valueStream.WriteInt32(value.Value);
            }
        );
        stream.WriteArray(
            Pairs,
            static (valueStream, value) =>
            {
                valueStream.WriteVarInt(value.Key);
                valueStream.WriteVarInt(value.Value);
            }
        );
        stream.WriteArray(
            UnknownValues0,
            static (valueStream, value) => valueStream.WriteVarInt(value)
        );
        stream.WriteArray(
            UnknownValues1,
            static (valueStream, value) => valueStream.WriteVarInt(value)
        );
        stream.WriteArray(
            Strings,
            static (valueStream, value) =>
            {
                valueStream.WriteVarInt(value.Key);
                valueStream.WriteOptionalString(value.Value);
            }
        );
        stream.WriteArray(
            UnknownEntries0,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteArray(
            UnknownEntries1,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteArray(
            Triples,
            static (valueStream, value) =>
            {
                valueStream.WriteVarInt(value.Unknown0);
                valueStream.WriteVarInt(value.Unknown1);
                valueStream.WriteVarInt(value.Unknown2);
            }
        );
    }
}
