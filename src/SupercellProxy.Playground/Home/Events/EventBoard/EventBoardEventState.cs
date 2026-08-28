namespace SupercellProxy.Playground.Home;

internal sealed class EventBoardEventState(
    int eventId,
    int variantId,
    bool seenInEventBoard,
    bool hasLinkedEventState
)
{
    public int EventId { get; } = eventId;
    public int VariantId { get; } = variantId;
    public bool SeenInEventBoard { get; private set; } = seenInEventBoard;
    public bool HasLinkedEventState { get; } = hasLinkedEventState;

    public bool TryMarkSeen()
    {
        if (!HasLinkedEventState || SeenInEventBoard)
            return false;

        SeenInEventBoard = true;
        return true;
    }
}
