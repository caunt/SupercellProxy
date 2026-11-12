using System.Globalization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Identifies the native result of a follow relationship operation.</summary>
public readonly record struct FollowResultCode
{
    /// <summary>Initializes a new instance of the <see cref="FollowResultCode"/> struct. Creates a result code from its native wire value.</summary>
    /// <param name="value">The result code as read from the follow response; any integer is kept, including codes that have no named instance.</param>
    public FollowResultCode(int value)
    {
        Value = value;
    }

    /// <summary>Gets the successful result.</summary>
    /// <value>The result with wire value 1.</value>
    public static FollowResultCode Success { get; } = new(value: 1);

    /// <summary>Gets the result reported when the farm cannot be followed.</summary>
    /// <value>The result with wire value 3, reported for a failed follow operation.</value>
    public static FollowResultCode CannotFollowFarm { get; } = new(value: 3);

    /// <summary>Gets the result reported when the target cannot accept another follower.</summary>
    /// <value>The result with wire value 4, reported for a follow operation when the target farm's follower limit is reached.</value>
    public static FollowResultCode TargetHasTooManyFollowers { get; } = new(value: 4);

    /// <summary>Gets the native wire value.</summary>
    /// <value>The integer written as a variable-length integer at the start of a follow response: 1 success, 3 cannot follow, 4 too many followers; other codes are not identified.</value>
    public int Value { get; }

    /// <summary>Formats the known result name or its numeric wire value.</summary>
    /// <returns><c>Success</c>, <c>CannotFollowFarm</c> or <c>TargetHasTooManyFollowers</c> for the named results; otherwise, <see cref="Value"/> in invariant-culture digits.</returns>
    public override string ToString()
    {
        return this == Success
            ? nameof(Success)
            : this == CannotFollowFarm
            ? nameof(CannotFollowFarm)
            : this == TargetHasTooManyFollowers
            ? nameof(TargetHasTooManyFollowers)
            : Value.ToString(CultureInfo.InvariantCulture);
    }
}
