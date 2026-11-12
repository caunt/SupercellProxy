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

    /// <summary>Initializes a new instance of the <see cref="MapGameGasStationTaskStatePayload"/> record. Creates the state from its decoded fields.</summary>
    /// <param name="freeFuel">Whether the station's fuel is free.</param>
    /// <param name="fuelCollected">Whether the fuel was already taken.</param>
    /// <param name="diamondPrice">Diamonds the fuel costs when it is not free.</param>
    /// <param name="fuelData">Fuel the station gives; <see langword="null"/> when not drawn yet.</param>
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
    public static ICodec<MapGameGasStationTaskStatePayload> Codec { get; } = Codecs.Composite(Codecs.Boolean,
        static value => value.FreeFuel,
        Codecs.Boolean,
        static value => value.FuelCollected,
        Codecs.VarInt,
        static value => value.DiamondPrice,
        Codecs.OptionalValue(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount)),
        static value => value.FuelData,
        static (freeFuel, fuelCollected, diamondPrice, fuelData) => new MapGameGasStationTaskStatePayload(freeFuel, fuelCollected, diamondPrice, fuelData));

    /// <summary>
    /// Gets the diamond price of buying the fuel.
    /// </summary>
    /// <value>Diamonds the player pays to take the fuel when <see cref="FreeFuel"/> is <see langword="false"/>; sent as a variable-length integer after the two flags.</value>
    public int DiamondPrice { get; }

    /// <summary>
    /// Gets a value indicating whether the fuel is free.
    /// </summary>
    /// <value><see langword="true"/> if the fuel can be taken without paying; otherwise, <see langword="false"/>.</value>
    public bool FreeFuel { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the fuel was collected.
    /// </summary>
    /// <value><see langword="true"/> if a player has taken the station's fuel; otherwise, <see langword="false"/>.</value>
    public bool FuelCollected { get; init; }

    /// <summary>
    /// Gets the fuel the station grants, or <see langword="null"/> before it is generated.
    /// </summary>
    /// <value>Pairs of a resource row's global id and amount, ordered by id.</value>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? FuelData { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
