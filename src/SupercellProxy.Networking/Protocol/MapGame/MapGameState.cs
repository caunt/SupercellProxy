using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Native map-game state structure encoded by the shared 1.72.84 helper at 0x10064b114.</para>
/// </summary>
public sealed record MapGameState : ICodecProvider<MapGameState>
{
    /// <summary>
    /// Initializes a new <see cref="MapGameState"/> instance.
    /// </summary>
    public MapGameState(
        ReadOnlyMemory<MapGamePawn> pawns,
        ReadOnlyMemory<MapGameTaskGroup> taskGroups,
        MapGameConfiguration configuration,
        ReadOnlyMemory<MapGameNodeEmoji> emojis,
        int mapGlobalId
    )
    {
        Pawns = pawns.ToArray();
        TaskGroups = taskGroups.ToArray();
        Configuration = configuration;
        Emojis = emojis.ToArray();
        MapGlobalId = mapGlobalId;
    }

    /// <inheritdoc />
    public static ICodec<MapGameState> Codec { get; } = Codecs.Composite(
        Codecs.Memory(MapGamePawn.Codec),
        static state => state.Pawns,
        Codecs.Memory(MapGameTaskGroup.Codec),
        static state => state.TaskGroups,
        MapGameConfiguration.Codec,
        static state => state.Configuration,
        Codecs.Memory(MapGameNodeEmoji.Codec),
        static state => state.Emojis,
        Codecs.VarInt,
        static state => state.MapGlobalId,
        static (pawns, taskGroups, configuration, emojis, mapGlobalId) => new MapGameState(pawns, taskGroups, configuration, emojis, mapGlobalId)
    );

    /// <summary>
    /// Gets the <c language="csharp">Configuration</c> value.
    /// </summary>
    public MapGameConfiguration Configuration { get; }

    /// <summary>
    /// Gets the <c language="csharp">Emojis</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Entries")]
    public ReadOnlyMemory<MapGameNodeEmoji> Emojis { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">MapGlobalId</c> value.
    /// </summary>
    public int MapGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Pawns</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGamePawn> Pawns { get; }

    /// <summary>
    /// Gets the <c language="csharp">TaskGroups</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTaskGroup> TaskGroups { get; }
}
