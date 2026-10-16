using System.Globalization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Identifies the relationship operation acknowledged by a follow response.</summary>
public readonly record struct FollowOperation
{
    /// <summary>Creates an operation from its native wire value.</summary>
    public FollowOperation(int value)
    {
        Value = value;
    }

    /// <summary>Gets the operation that begins following a farm.</summary>
    public static FollowOperation Follow { get; } = new(value: 1);

    /// <summary>Gets the operation that removes a farm from the following list.</summary>
    public static FollowOperation Unfollow { get; } = new(value: 2);

    /// <summary>Gets the operation that removes a farm from the follower list.</summary>
    public static FollowOperation RemoveFollower { get; } = new(value: 4);

    /// <summary>Gets the native wire value.</summary>
    public int Value { get; }

    /// <summary>Formats the known operation name or its numeric wire value.</summary>
    public override string ToString()
    {
        return this == Follow
            ? nameof(Follow)
            : this == Unfollow
            ? nameof(Unfollow)
            : this == RemoveFollower
            ? nameof(RemoveFollower)
            : Value.ToString(CultureInfo.InvariantCulture);
    }
}
