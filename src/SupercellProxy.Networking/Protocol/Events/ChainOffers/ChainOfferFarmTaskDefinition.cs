namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Describes gameplay progress needed for a chain reward.</summary>
public sealed record ChainOfferFarmTaskDefinition
{
    /// <summary>Gets or sets the required progress.</summary>
    /// <value>How many matching task events the step needs, such as production starts.</value>
    public int Amount { get; init; }

    /// <summary>Gets or sets the optional product or building filter.</summary>
    /// <value>The data-table name the task's subject must have, or <see langword="null"/> or empty to count every subject of <see cref="Type"/>.</value>
    public string? Good { get; init; }

    /// <summary>Gets or sets the native task type.</summary>
    /// <value>The task type name as the event document writes it, or <see langword="null"/> when absent.</value>
    public string? Type { get; init; }
}
