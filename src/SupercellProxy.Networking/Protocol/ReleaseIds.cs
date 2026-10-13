using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol;

/// <summary>The wire ids one release adds, changes or removes, as input to a <see cref="ProtocolIdRegistry{TContract}"/>.</summary>
/// <typeparam name="TContract">The contract key.</typeparam>
/// <param name="Version">The release version.</param>
/// <param name="Changes">Each changed contract with its new id, or <see langword="null"/> when the release removes it.</param>
internal sealed record ReleaseIds<TContract>(Version Version, ImmutableArray<KeyValuePair<TContract, int?>> Changes);
