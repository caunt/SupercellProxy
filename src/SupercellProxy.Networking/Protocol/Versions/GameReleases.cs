using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.Versions;

/// <summary>Every game release with its own id table, oldest first; a new release is added here after its table.</summary>
internal static class GameReleases
{
    /// <summary>Gets the releases, oldest first.</summary>
    internal static ImmutableArray<GameRelease> All { get; } = [Release17286.Release, Release17381.Release];
}
