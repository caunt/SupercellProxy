using System.Globalization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Identifies the relationship operation acknowledged by a follow response.</summary>
public readonly record struct FollowOperation
{
    /// <summary>Initializes a new instance of the <see cref="FollowOperation"/> struct. Creates an operation from its native wire value.</summary>
    /// <param name="value">The operation code as read from the follow response; any integer is kept, including codes that have no named instance.</param>
    public FollowOperation(int value)
    {
        Value = value;
    }

    /// <summary>Gets the operation that begins following a farm.</summary>
    /// <value>The operation with wire value 1, which a <see cref="FollowMessage"/> asks for.</value>
    public static FollowOperation Follow { get; } = new(value: 1);

    /// <summary>Gets the operation that removes a farm from the following list.</summary>
    /// <value>The operation with wire value 2, which an <see cref="UnfollowMessage"/> asks for.</value>
    public static FollowOperation Unfollow { get; } = new(value: 2);

    /// <summary>Gets the operation that removes a farm from the follower list.</summary>
    /// <value>The operation with wire value 4.</value>
    public static FollowOperation RemoveFollower { get; } = new(value: 4);

    /// <summary>Gets the native wire value.</summary>
    /// <value>The integer written as a variable-length integer in a follow response: 1 follow, 2 unfollow, 4 remove follower; other codes are not identified.</value>
    public int Value { get; }

    /// <summary>Formats the known operation name or its numeric wire value.</summary>
    /// <returns><c>Follow</c>, <c>Unfollow</c> or <c>RemoveFollower</c> for the named operations; otherwise, <see cref="Value"/> in invariant-culture digits.</returns>
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
