using System.Globalization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Identifies the relationship operation acknowledged by a follow response.
public readonly record struct FollowOperation
{
    /// Creates an operation from its native wire value.
    public FollowOperation(int value)
    {
        Value = value;
    }

    /// Gets the operation that begins following a farm.
    public static FollowOperation Follow { get; } = new(value: 1);

    /// Gets the operation that removes a farm from the following list.
    public static FollowOperation Unfollow { get; } = new(value: 2);

    /// Gets the operation that removes a farm from the follower list.
    public static FollowOperation RemoveFollower { get; } = new(value: 4);

    /// Gets the native wire value.
    public int Value { get; }

    /// Formats the known operation name or its numeric wire value.
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
