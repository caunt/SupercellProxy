using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameObstacleTaskStatePayload</c>.
/// </summary>
public sealed record MapGameObstacleTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameObstacleTaskStatePayload"/> instance.
    /// </summary>
    public MapGameObstacleTaskStatePayload(
        bool unknownBoolean0,
        bool unknownBoolean1,
        LongId? unknownLongId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? optionalValues
    )
    {
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        UnknownLongId = unknownLongId;
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
    /// Gets the <c>UnknownLongId</c> value.
    /// </summary>
    public LongId? UnknownLongId { get; }

    /// <summary>
    /// Gets the <c>OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? OptionalValues { get; }

    internal static MapGameObstacleTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameObstacleTaskStatePayload(
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            MapGameWire.ReadOptionalLongId(stream),
            MapGameWire.ReadOptionalDataReferenceVarIntPairs(stream)
        );
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId);
        MapGameWire.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
    }
}
