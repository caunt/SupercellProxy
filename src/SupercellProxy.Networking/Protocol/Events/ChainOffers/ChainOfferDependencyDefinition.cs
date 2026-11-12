namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Describes a timed or gameplay requirement for a chain step.</summary>
public sealed record ChainOfferDependencyDefinition
{
    /// <summary>Gets or sets the gameplay action requirement.</summary>
    /// <value>The task the player has to perform when <see cref="Type"/> is <c>2</c>; <see langword="null"/> for other requirement types.</value>
    public ChainOfferFarmTaskDefinition? FarmTask { get; init; }
    /// <summary>Gets or sets the delay from the event's start.</summary>
    /// <value>The time that must pass after the event's start when <see cref="Type"/> is <c>1</c>; <see langword="null"/> for other requirement types.</value>
    public ChainOfferTimeDefinition? TimeSinceEventStart { get; init; }
    /// <summary>Gets or sets the native requirement discriminator.</summary>
    /// <value>The requirement kind as text: <c>1</c> time since the event start, <c>2</c> a farm task, <c>3</c> seconds since the previous step's claim, <c>4</c> calendar days since the previous step's claim.</value>
    public string? Type { get; init; }
}
