using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// A gas station task state, saved natively with the keys <c>FreeFuel</c>, <c>FuelCollected</c>, <c>DiamondPrice</c> and <c>FuelData</c>.
/// </summary>
public sealed record MapGameGasStationTaskStatePayload : MapGameTaskStatePayload, ICodecProvider<MapGameGasStationTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Gas Station";

    /// <summary>Creates the state from its decoded fields.</summary>
    /// <param name="freeFuel">Whether the station's fuel is free.</param>
    /// <param name="fuelCollected">Whether the fuel was already taken.</param>
    /// <param name="diamondPrice">Diamonds the fuel costs when it is not free.</param>
    /// <param name="fuelData">Fuel the station gives; null when not drawn yet.</param>
    public MapGameGasStationTaskStatePayload(bool freeFuel, bool fuelCollected, int diamondPrice, ReadOnlyMemory<CommandDataReferenceVarIntPair>? fuelData)
    {
        FreeFuel = freeFuel;
        FuelCollected = fuelCollected;
        DiamondPrice = diamondPrice;
        FuelData = fuelData is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)fuelData.Value.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameGasStationTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static value => value.FreeFuel,
        Codecs.Boolean,
        static value => value.FuelCollected,
        Codecs.VarInt,
        static value => value.DiamondPrice,
        Codecs.OptionalValue(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount)),
        static value => value.FuelData,
        static (freeFuel, fuelCollected, diamondPrice, fuelData) => new MapGameGasStationTaskStatePayload(freeFuel, fuelCollected, diamondPrice, fuelData)
    );

    /// <summary>
    /// Gets the diamond price of buying the fuel.
    /// </summary>
    public int DiamondPrice { get; }

    /// <summary>
    /// Gets a value indicating whether the fuel is free.
    /// </summary>
    public bool FreeFuel { get; }

    /// <summary>
    /// Gets a value indicating whether the fuel was collected.
    /// </summary>
    public bool FuelCollected { get; init; }

    /// <summary>
    /// Gets the fuel the station grants, or <see langword="null"/> before it is generated.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? FuelData { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
