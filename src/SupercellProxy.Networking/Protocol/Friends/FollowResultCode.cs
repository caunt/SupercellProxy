using System.Globalization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Identifies the native result of a follow relationship operation.</summary>
public readonly record struct FollowResultCode
{
    /// <summary>Creates a result code from its native wire value.</summary>
    public FollowResultCode(int value)
    {
        Value = value;
    }

    /// <summary>Gets the successful result.</summary>
    public static FollowResultCode Success { get; } = new(value: 1);

    /// <summary>Gets the result reported when the farm cannot be followed.</summary>
    public static FollowResultCode CannotFollowFarm { get; } = new(value: 3);

    /// <summary>Gets the result reported when the target cannot accept another follower.</summary>
    public static FollowResultCode TargetHasTooManyFollowers { get; } = new(value: 4);

    /// <summary>Gets the native wire value.</summary>
    public int Value { get; }

    /// <summary>Formats the known result name or its numeric wire value.</summary>
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
