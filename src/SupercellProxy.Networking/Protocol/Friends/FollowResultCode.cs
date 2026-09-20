using System.Globalization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Identifies the native result of a follow relationship operation.
public readonly record struct FollowResultCode
{
    /// Creates a result code from its native wire value.
    public FollowResultCode(int value)
    {
        Value = value;
    }

    /// Gets the successful result.
    public static FollowResultCode Success { get; } = new(value: 1);

    /// Gets the result reported when the farm cannot be followed.
    public static FollowResultCode CannotFollowFarm { get; } = new(value: 3);

    /// Gets the result reported when the target cannot accept another follower.
    public static FollowResultCode TargetHasTooManyFollowers { get; } = new(value: 4);

    /// Gets the native wire value.
    public int Value { get; }

    /// Formats the known result name or its numeric wire value.
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
