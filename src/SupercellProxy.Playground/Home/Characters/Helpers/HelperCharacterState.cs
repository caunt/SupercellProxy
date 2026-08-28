using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record HelperCharacterState(
    GameObjectState GameObject,
    int ChecksumState0,
    PathState Path,
    int ChecksumState1,
    int ChecksumState2,
    int ChecksumState3
)
{
    public static HelperCharacterState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryGetTableId(
                GameAssetFiles.HelperCharacters,
                out var helperCharacterTableId
            )
        )
            throw new InvalidOperationException(
                $"{GameAssetFiles.HelperCharacters} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == helperCharacterTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver))
            .ToArray();
    }

    private static HelperCharacterState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "MaxPathLength",
                out var maximumPathLength
            )
        )
            throw new InvalidDataException(
                $"Helper character {gameObject.Data.Name} has no MaxPathLength value."
            );

        return new HelperCharacterState(
            gameObject,
            0,
            PathState.CreateIdle(maximumPathLength),
            0,
            0,
            0
        );
    }
}
