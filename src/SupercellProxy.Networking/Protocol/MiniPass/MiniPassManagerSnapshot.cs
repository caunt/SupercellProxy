namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved current Mini Pass and its active tasks.</summary>
public sealed record MiniPassManagerSnapshot
{
    /// <summary>Gets the current Mini Pass when one is present.</summary>
    public MiniPassInstanceSnapshot? Current { get; init; }

    /// <summary>Gets the saved task slots belonging to the current pass.</summary>
    public MiniPassTaskSnapshot[] Tasks { get; init; } = [];
}
