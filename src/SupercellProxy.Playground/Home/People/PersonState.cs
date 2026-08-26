using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record PersonState(
    GameObjectState GameObject,
    int MovementSpeed,
    int IdleTime,
    int State,
    int ChecksumState0,
    int NextPoint,
    int GoodAmount,
    DataTableReference Good,
    int ChecksumState1,
    int ChecksumState2,
    int PaymentObjectAmount,
    DataTableReference PaymentObject,
    int ChecksumState3,
    bool ChecksumFlag0,
    bool ChecksumFlag1,
    IntPair ChecksumPair0,
    IntPair ChecksumPair1,
    IntPair ChecksumPair2,
    bool HasParent,
    IntPair[]? ChecksumPoints0,
    IntPair[]? ChecksumPoints1,
    DataTableReference? ChecksumData
)
{
    public static PersonState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string peopleFile = "data/people.csv";

        if (!dataTableResolver.TryGetTableId(peopleFile, out var peopleTableId))
            throw new InvalidOperationException(
                $"{peopleFile} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == peopleTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver))
            .ToArray();
    }

    public static void Update(PersonState[] people, PersonRouteState routes, GameRandom random)
    {
        for (var i = 0; i < people.Length; i++)
            people[i] = people[i].UpdateOne(people, routes, random);
    }

    public PersonState CompletePostLoadSetup(int experienceMultiplier, int constantExperience)
    {
        if (experienceMultiplier is not 0)
        {
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Person post-load setup with experience multiplier {experienceMultiplier} is not implemented."
                )
            );
        }

        return this with
        {
            ChecksumState3 = constantExperience,
            ChecksumFlag1 = true,
        };
    }

    private static PersonState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        var snapshot = gameObject.Snapshot;

        if (
            snapshot.State is not 1
            || snapshot.PeopleQuestV2.ValueKind
                is not JsonValueKind.Undefined
                    and not JsonValueKind.Null
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Person {gameObject.GlobalId} is not in the native roadside-shop visitor state."
                )
            );
        }

        if (!dataTableResolver.TryResolve(snapshot.GoodGlobalId, out var good))
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Person {gameObject.GlobalId} has unresolved GoodGlobalId {snapshot.GoodGlobalId}."
                )
            );

        var paymentObject = ResolvePaymentObject(gameObject, dataTableResolver);
        var (movementSpeed, idleTime) = ResolveMovementConfiguration(gameObject, dataTableResolver);

        return new PersonState(
            gameObject,
            movementSpeed,
            idleTime,
            snapshot.State,
            0,
            snapshot.NextPoint,
            snapshot.GoodAmount,
            good,
            -1,
            -1,
            snapshot.PaymentObjectAmount,
            paymentObject,
            0,
            ChecksumFlag0: false,
            ChecksumFlag1: false,
            default,
            default,
            default,
            HasParent: false,
            ChecksumPoints0: null,
            ChecksumPoints1: null,
            ChecksumData: null
        );
    }

    private static DataTableReference ResolvePaymentObject(
        GameObjectState gameObject,
        DataTableResolver resolver
    )
    {
        return resolver.TryResolve(gameObject.Snapshot.PaymentObjectGlobalId, out var paymentObject)
            ? paymentObject
            : throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Person {gameObject.GlobalId} has unresolved PaymentObjectGlobalId {gameObject.Snapshot.PaymentObjectGlobalId}."
                )
            );
    }

    private static (int MovementSpeed, int IdleTime) ResolveMovementConfiguration(
        GameObjectState gameObject,
        DataTableResolver resolver
    )
    {
        if (
            !resolver.TryResolveInt(gameObject.Data.GlobalId, "WalkSpeed", out var movementSpeed)
            || !resolver.TryResolveInt(gameObject.Data.GlobalId, "IdleTime", out var idleTime)
            || movementSpeed < 0
            || idleTime < 0
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Person {gameObject.GlobalId} has incomplete native movement configuration."
                )
            );
        return (movementSpeed, idleTime);
    }

    internal PersonState UpdateOne(PersonState[] people, PersonRouteState routes, GameRandom random)
    {
        if (!ChecksumFlag1)
            return this;

        if (State is not 1)
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Person state {State} update is not implemented."
                )
            );

        var timer = unchecked(ChecksumState0 - 1);
        var targetX = ChecksumState1;
        var targetY = ChecksumState2;

        if (targetX is -1 && timer < 1)
            (targetX, targetY) = SelectTarget(people, routes, random);

        if (targetX is -1)
            return this with { ChecksumState0 = timer };

        var currentX = GameObject.PositionX;
        var currentY = GameObject.PositionY;
        var movementX = unchecked(targetX - currentX);
        var movementY = unchecked(targetY - currentY);
        var distance = IntegerMath.GetVectorLength(movementX, movementY);

        if (MovementSpeed < distance)
        {
            movementX = unchecked(movementX * MovementSpeed) / distance;
            movementY = unchecked(movementY * MovementSpeed) / distance;
        }
        else
        {
            timer = unchecked(IdleTime + IdleTime * random.NextInt(3));
            targetX = -1;
        }

        if (movementX != movementY)
            GameObject.SetMirrored(movementX <= movementY);

        GameObject.MoveTo(unchecked(currentX + movementX), unchecked(currentY + movementY));

        return this with
        {
            ChecksumState0 = timer,
            ChecksumState1 = targetX,
            ChecksumState2 = targetY,
        };
    }

    private (int X, int Y) SelectTarget(
        PersonState[] people,
        PersonRouteState routes,
        GameRandom random
    )
    {
        const int InitialSeparation = 0x2aa;
        const int RelaxedSeparation = 0x155;
        const int RelaxedAttempt = 150;
        const int MaximumAttempts = 300;

        var entry = routes.EntryRoute[^1];
        var exit = routes.ExitRoute[0];
        var rangeX = checked(exit.First - entry.First + 0x280);

        if (rangeX < 1)
            throw new InvalidDataException(
                "The native people routes do not define a positive visitor target range."
            );

        var targetX = 0;
        var targetY = 0;

        for (var attempt = 0; attempt < MaximumAttempts; attempt++)
        {
            targetX = unchecked(entry.First - 0x200 + random.NextInt(rangeX));
            targetY = unchecked(entry.Second - 0x533 + random.NextInt(0x533));
            var minimumSeparation =
                attempt < RelaxedAttempt ? InitialSeparation : RelaxedSeparation;
            var separated = true;

            foreach (var person in people)
            {
                if (ReferenceEquals(person.GameObject, GameObject))
                    continue;

                var deltaX = unchecked(targetX - person.ChecksumState1);
                var deltaY = unchecked(targetY - person.ChecksumState2);

                if (IntegerMath.GetVectorLength(deltaX, deltaY) >= minimumSeparation)
                    continue;

                separated = false;
                break;
            }

            if (separated)
                break;
        }

        return (targetX, targetY);
    }
}
