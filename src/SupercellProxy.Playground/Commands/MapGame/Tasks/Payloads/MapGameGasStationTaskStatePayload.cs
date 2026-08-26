using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameGasStationTaskStatePayload</c>.
/// </summary>
public sealed record MapGameGasStationTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameGasStationTaskStatePayload"/> instance.
    /// </summary>
    public MapGameGasStationTaskStatePayload(
        bool unknownBoolean0,
        bool unknownBoolean1,
        int unknown0,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? optionalValues
    )
    {
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        Unknown0 = unknown0;
        OptionalValues = optionalValues?.ToArray();
    }

    /// <summary>
    /// Gets the <c>UnknownBoolean0</c> value.
    /// </summary>
    public bool UnknownBoolean0 { get; }

    /// <summary>
    /// Gets the <c>UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; }

    /// <summary>
    /// Gets the <c>Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c>OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? OptionalValues { get; }

    internal static MapGameGasStationTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameGasStationTaskStatePayload(
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            MapGameWire.ReadOptionalDataReferenceVarIntPairs(stream)
        );
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteVarInt(Unknown0);
        MapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
    }
}
