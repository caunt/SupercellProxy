using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SupercellProxy.Networking.Protocol;

/// <summary>
/// <para>Maps protocol contracts to the wire id each game release uses for them, and wire ids back to contracts.</para>
/// <para>Releases are applied in order, each overriding only the ids it changes. A client release without its own table uses the newest older release.</para>
/// <para>When the game version is unknown, an id resolves only if every release agrees on it.</para>
/// </summary>
/// <typeparam name="TContract">The contract key.</typeparam>
public sealed class ProtocolIdRegistry<TContract> where TContract : notnull
{
    private readonly ImmutableArray<FrozenDictionary<int, TContract>> _contractsByRelease;
    private readonly ImmutableArray<FrozenDictionary<TContract, int>> _idsByRelease;

    internal ProtocolIdRegistry(IEnumerable<ReleaseIds<TContract>> releases)
    {
        ArgumentNullException.ThrowIfNull(releases);

        Dictionary<TContract, int> current = [];
        List<TContract> contracts = [];
        List<Version> versions = [];
        List<FrozenDictionary<TContract, int>> idsByRelease = [];
        List<FrozenDictionary<int, TContract>> contractsByRelease = [];
        Version? previous = null;

        foreach (ReleaseIds<TContract> release in releases)
        {
            if (previous is not null && release.Version <= previous)
                throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"Release {release.Version} is not newer than {previous}."));

            foreach ((TContract contract, int? id) in release.Changes)
            {
                contracts.Add(contract);

                if (id is { } value)
                    current[contract] = value;
                else if (!current.Remove(contract))
                    throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"Release {release.Version} removes {contract}, which no earlier release has."));
            }

            versions.Add(release.Version);
            previous = release.Version;
            idsByRelease.Add(current.ToFrozenDictionary());
            contractsByRelease.Add(IndexContracts(release.Version, current));
        }

        Contracts = [.. contracts.Distinct()];
        Releases = [.. versions];
        _idsByRelease = [.. idsByRelease];
        _contractsByRelease = [.. contractsByRelease];
    }

    /// <summary>Gets every contract any release assigns.</summary>
    public ImmutableArray<TContract> Contracts { get; }

    /// <summary>Gets the versions of the releases with their own id tables, oldest first.</summary>
    public ImmutableArray<Version> Releases { get; }

    /// <summary>Gets the contract that owns a wire id in a game release.</summary>
    public TContract GetContract(int id, Version? gameVersion)
    {
        return TryGetContract(id, gameVersion, out TContract? contract)
            ? contract
            : throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Id {id} is not registered for game version {gameVersion}."));
    }

    /// <summary>Gets the wire id of a contract in a game release.</summary>
    public int GetId(TContract contract, Version? gameVersion)
    {
        return TryGetId(contract, gameVersion, out int id)
            ? id
            : throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"{contract} is not registered for game version {gameVersion}."));
    }

    /// <summary>Gets every wire id a contract has used across releases.</summary>
    public IEnumerable<int> GetIds(TContract contract)
    {
        return _idsByRelease.Select(ids => ids.TryGetValue(contract, out int id) ? id : (int?)null).OfType<int>().Distinct();
    }

    /// <summary>Checks whether a wire id belongs to a contract in a game release.</summary>
    public bool Is(int id, TContract contract, Version? gameVersion)
    {
        return TryGetContract(id, gameVersion, out TContract? owner) && EqualityComparer<TContract>.Default.Equals(owner, contract);
    }

    /// <summary>Resolves the contract that owns a wire id in a game release.</summary>
    public bool TryGetContract(int id, Version? gameVersion, [MaybeNullWhen(returnValue: false)] out TContract contract)
    {
        if (gameVersion is not null)
        {
            int release = FindRelease(gameVersion);
            contract = default;

            return release >= 0 && _contractsByRelease[release].TryGetValue(id, out contract);
        }

        TContract[] owners = DistinctValues(_contractsByRelease, id);

        if (owners.Length > 1)
        {
            throw new InvalidOperationException(
                string.Create(CultureInfo.InvariantCulture, $"Id {id} belongs to different contracts across releases; the game version is required.")
            );
        }

        if (owners is [TContract owner])
        {
            contract = owner;

            return true;
        }

        contract = default;

        return false;
    }

    /// <summary>Gets the wire id of a contract in a game release when it is registered there.</summary>
    public bool TryGetId(TContract contract, Version? gameVersion, out int id)
    {
        if (gameVersion is not null)
        {
            int release = FindRelease(gameVersion);
            id = 0;

            return release >= 0 && _idsByRelease[release].TryGetValue(contract, out id);
        }

        int[] ids = DistinctValues(_idsByRelease, contract);

        if (ids.Length > 1)
            throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"{contract} has different ids across releases; the game version is required."));

        id = ids is [int only] ? only : 0;

        return ids.Length is not 0;
    }

    // Without a game version a key resolves only when every release that knows it agrees on its value.
    private static TValue[] DistinctValues<TKey, TValue>(ImmutableArray<FrozenDictionary<TKey, TValue>> releases, TKey key)
        where TKey : notnull
    {
        List<TValue> values = [];

        foreach (FrozenDictionary<TKey, TValue> release in releases)
        {
            if (release.TryGetValue(key, out TValue? value) && !values.Contains(value))
                values.Add(value);
        }

        return [.. values];
    }

    private static FrozenDictionary<int, TContract> IndexContracts(Version version, Dictionary<TContract, int> ids)
    {
        Dictionary<int, TContract> contracts = [];

        foreach ((TContract contract, int id) in ids)
        {
            if (!contracts.TryAdd(id, contract))
                throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"Release {version} assigns id {id} to both {contracts[id]} and {contract}."));
        }

        return contracts.ToFrozenDictionary();
    }

    private int FindRelease(Version gameVersion)
    {
        int release = -1;

        for (int index = 0; index < Releases.Length && Releases[index] <= gameVersion; index++)
            release = index;

        return release;
    }
}
