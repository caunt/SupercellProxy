using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record CarState(
    GameObjectState GameObject,
    int State,
    int ChecksumState0,
    int ChecksumState1,
    int RewardAmount,
    int RewardCount,
    int RewardType,
    CarPathState Path0,
    CarPathState Path1
)
{
    public static CarState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Cars, out var carTableId))
            throw new InvalidOperationException(
                $"{GameAssetFiles.Cars} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == carTableId)
            .Select(Create)
            .ToArray();
    }

    private static CarState Create(GameObjectState gameObject)
    {
        var snapshot = gameObject.Snapshot;

        if (snapshot.State is not 1)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Car {gameObject.GlobalId} has unsupported state {snapshot.State}."
                )
            );

        if (
            snapshot.Data.TryGetValue("rewards", out var rewards)
            && (rewards.ValueKind is not JsonValueKind.Array || rewards.GetArrayLength() is not 0)
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Car {gameObject.GlobalId} has unsupported rewards."
                )
            );
        }

        ValidatePosition(gameObject);

        var x = gameObject.PositionX >> 9;
        var y = gameObject.PositionY >> 9;

        return new CarState(
            gameObject,
            snapshot.State,
            0,
            0,
            0,
            0,
            0,
            CarPathState.Create(
                PackPoint(x, y),
                PackPoint(x, 32),
                PackPoint(x - 3, 35),
                PackPoint(-15, 35)
            ),
            CarPathState.Create(
                PackPoint(x + 13, 3),
                PackPoint(x + 13, 38),
                PackPoint(x + 10, 35),
                PackPoint(x + 3, 35),
                PackPoint(x, 32),
                PackPoint(x, y)
            )
        );
    }

    private static void ValidatePosition(GameObjectState gameObject)
    {
        if ((gameObject.PositionX & 0x1ff) is not 0 || (gameObject.PositionY & 0x1ff) is not 0)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Car {gameObject.GlobalId} has an inaccurate native map position."
                )
            );
    }

    private static int PackPoint(int x, int y)
    {
        if (x is < -0x7fff or > 0x7fff)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Car path X coordinate {x} is outside the native range."
                )
            );

        var encodedX = Math.Abs(x) | (x < 0 ? 0x8000 : 0);
        return unchecked(encodedX | (y << 16));
    }
}
