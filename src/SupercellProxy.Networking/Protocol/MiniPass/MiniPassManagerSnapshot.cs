using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved current Mini Pass and its active tasks.</summary>
public sealed record MiniPassManagerSnapshot
{
    /// <summary>Gets or sets the current Mini Pass when one is present.</summary>
    /// <value>The running or most recent pass with its points and reward tracks, or <see langword="null"/> when the home has no Mini Pass.</value>
    public MiniPassInstanceSnapshot? Current { get; init; }

    /// <summary>Gets or sets the saved task slots belonging to the current pass.</summary>
    /// <value>One entry per task that earns pass points; empty by default. Task progress itself is not saved here.</value>
    public ImmutableArray<MiniPassTaskSnapshot> Tasks { get; init; } = [];
}
