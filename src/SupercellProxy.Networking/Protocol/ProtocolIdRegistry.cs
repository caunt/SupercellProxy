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

        // The live id table that each release patches with its own changes.
        Dictionary<TContract, int> current = [];

        // What the registry keeps from every release for its listings and lookups.
        List<TContract> contracts = [];
        List<Version> versions = [];
        List<FrozenDictionary<TContract, int>> idsByRelease = [];
        List<FrozenDictionary<int, TContract>> contractsByRelease = [];

        // The last release seen, so an out-of-order one is rejected.
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
    /// <value>Each contract once, in the order releases first mention it, including contracts a later release removes.</value>
    public ImmutableArray<TContract> Contracts { get; }

    /// <summary>Gets the versions of the releases with their own id tables, oldest first.</summary>
    /// <value>Strictly increasing versions; a client version between two entries uses the table of the older one.</value>
    public ImmutableArray<Version> Releases { get; }

    /// <summary>Gets the contract that owns a wire id in a game release.</summary>
    /// <param name="id">The wire id read from a frame header or command payload.</param>
    /// <param name="gameVersion">
    /// The client's game release; the newest release at or before it decides. With <see langword="null"/>, every release that uses the id
    /// must give it to the same contract.
    /// </param>
    /// <returns>The contract that owns <paramref name="id"/> in the release.</returns>
    /// <exception cref="NotSupportedException">
    /// No contract owns <paramref name="id"/> in the release, or <paramref name="gameVersion"/> is older than every release.
    /// </exception>
    public TContract GetContract(int id, Version? gameVersion)
    {
        return TryGetContract(id, gameVersion, out TContract? contract)
            ? contract
            : throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Id {id} is not registered for game version {gameVersion}."));
    }

    /// <summary>Gets the wire id of a contract in a game release.</summary>
    /// <param name="contract">The message type or command contract to look up.</param>
    /// <param name="gameVersion">
    /// The client's game release; the newest release at or before it decides. With <see langword="null"/>, every release that knows the
    /// contract must agree on its id.
    /// </param>
    /// <returns>The wire id the release assigns to <paramref name="contract"/>.</returns>
    /// <exception cref="NotSupportedException">
    /// The release does not assign <paramref name="contract"/> an id: it was never added, was removed, or <paramref name="gameVersion"/> is
    /// older than every release.
    /// </exception>
    public int GetId(TContract contract, Version? gameVersion)
    {
        return TryGetId(contract, gameVersion, out int id)
            ? id
            : throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"{contract} is not registered for game version {gameVersion}."));
    }

    /// <summary>Gets every wire id a contract has used across releases.</summary>
    /// <param name="contract">The message type or command contract to look up.</param>
    /// <returns>The distinct ids in release order, oldest first; empty when no release assigns the contract.</returns>
    public IEnumerable<int> GetIds(TContract contract)
    {
        return _idsByRelease.Select(ids => ids.TryGetValue(contract, out int id) ? id : (int?)null).OfType<int>().Distinct();
    }

    /// <summary>Checks whether a wire id belongs to a contract in a game release.</summary>
    /// <param name="id">The wire id read from a frame header or command payload.</param>
    /// <param name="contract">The contract to compare the id's owner with.</param>
    /// <param name="gameVersion">
    /// The client's game release, or <see langword="null"/> to require that every release using the id gives it to the same contract.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="id"/> is assigned in the release and resolves to <paramref name="contract"/> there; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool Is(int id, TContract contract, Version? gameVersion)
    {
        return TryGetContract(id, gameVersion, out TContract? owner) && EqualityComparer<TContract>.Default
            .Equals(owner, contract);
    }

    /// <summary>Resolves the contract that owns a wire id in a game release.</summary>
    /// <param name="id">The wire id read from a frame header or command payload.</param>
    /// <param name="gameVersion">
    /// The client's game release; the newest release at or before it decides, and a version older than every release finds nothing. With
    /// <see langword="null"/>, the id resolves only when every release that uses it gives it to the same contract.
    /// </param>
    /// <param name="contract">When this method returns <see langword="true"/>, the contract that owns <paramref name="id"/>.</param>
    /// <returns><see langword="true"/> if a contract owns <paramref name="id"/> in the release; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="gameVersion"/> is <see langword="null"/> and different releases give <paramref name="id"/> to different contracts.
    /// </exception>
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
            throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"Id {id} belongs to different contracts across releases; the game version is required."));

        if (owners is [TContract owner])
        {
            contract = owner;

            return true;
        }

        contract = default;

        return false;
    }

    /// <summary>Gets the wire id of a contract in a game release when it is registered there.</summary>
    /// <param name="contract">The message type or command contract to look up.</param>
    /// <param name="gameVersion">
    /// The client's game release; the newest release at or before it decides, and a version older than every release finds nothing. With
    /// <see langword="null"/>, the id is taken only when every release that knows the contract agrees on it.
    /// </param>
    /// <param name="id">When this method returns <see langword="true"/>, the contract's wire id; otherwise, 0.</param>
    /// <returns><see langword="true"/> if the release assigns <paramref name="contract"/> an id; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="gameVersion"/> is <see langword="null"/> and releases give <paramref name="contract"/> different ids.
    /// </exception>
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
