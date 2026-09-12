namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Defines the Home Visit Failure Reason contract.
/// </summary>
public enum HomeVisitFailureReason
{
    /// <summary>
    /// Identifies the Cannot Return Home wire value.
    /// </summary>
    CannotReturnHome = 1,

    /// <summary>
    /// Identifies the Visit Unavailable wire value.
    /// </summary>
    VisitUnavailable = 2,
}
