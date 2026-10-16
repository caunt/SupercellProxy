namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Why the server refused to load a farm, as a home-load failure reports it.
/// </summary>
public enum HomeVisitFailureReason
{
    /// <summary>
    /// The player's own farm could not be loaded to return to.
    /// </summary>
    CannotReturnHome = 1,

    /// <summary>
    /// The farm to visit is not available, for example because no farm has that id.
    /// </summary>
    VisitUnavailable = 2,
}
