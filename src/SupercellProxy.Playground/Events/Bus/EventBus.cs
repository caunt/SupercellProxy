using Nito.AsyncEx;

namespace SupercellProxy.Playground.Events.Bus;

/// <summary>
/// Represents <c>EventBus</c>.
/// </summary>
public class EventBus
{
    private Delegate?[] eventDelegates = new Delegate?[16];
    private readonly AsyncLock subscriptionLock = new();

    /// <summary>
    /// Executes the <c>SubscribeAsync</c> operation.
    /// </summary>
    public async Task SubscribeAsync<TEvent>(
        Func<TEvent, CancellationToken, Task> asyncEventHandler,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        var eventIndex = EventTypeCache<TEvent>.Index;

        using var disposable = await subscriptionLock
            .LockAsync(cancellationToken)
            .ConfigureAwait(false);

        var currentDelegatesArray = Volatile.Read(ref eventDelegates);

        if (eventIndex >= currentDelegatesArray.Length)
        {
            var newArraySize = Math.Max(currentDelegatesArray.Length * 2, eventIndex + 1);
            var newDelegatesArray = new Delegate?[newArraySize];
            Array.Copy(currentDelegatesArray, newDelegatesArray, currentDelegatesArray.Length);

            newDelegatesArray[eventIndex] = asyncEventHandler;

            Volatile.Write(ref eventDelegates, newDelegatesArray);
        }
        else
        {
            var existingDelegate = Volatile.Read(ref currentDelegatesArray[eventIndex]);
            var updatedDelegate = Delegate.Combine(existingDelegate, asyncEventHandler);

            Volatile.Write(ref currentDelegatesArray[eventIndex], updatedDelegate);
        }
    }

    /// <summary>
    /// Executes the <c>UnsubscribeAsync</c> operation.
    /// </summary>
    public async Task UnsubscribeAsync<TEvent>(
        Func<TEvent, CancellationToken, Task> asyncEventHandler,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        var eventIndex = EventTypeCache<TEvent>.Index;

        using var disposable = await subscriptionLock
            .LockAsync(cancellationToken)
            .ConfigureAwait(false);

        var currentDelegatesArray = Volatile.Read(ref eventDelegates);

        if (eventIndex < currentDelegatesArray.Length)
        {
            var existingDelegate = Volatile.Read(ref currentDelegatesArray[eventIndex]);

            if (existingDelegate is not null)
            {
                var updatedDelegate = Delegate.Remove(existingDelegate, asyncEventHandler);
                Volatile.Write(ref currentDelegatesArray[eventIndex], updatedDelegate);
            }
        }
    }

    /// <summary>
    /// Executes the <c>PublishAsync</c> operation.
    /// </summary>
    public async Task<TEvent> PublishAsync<TEvent>(
        TEvent eventItem,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        var eventIndex = EventTypeCache<TEvent>.Index;
        var currentDelegatesArray = Volatile.Read(ref eventDelegates);

        if (eventIndex < currentDelegatesArray.Length)
        {
            var currentDelegates = Volatile.Read(ref currentDelegatesArray[eventIndex]);

            if (currentDelegates is not null)
            {
                var invocationList = currentDelegates.GetInvocationList();
                await Task.WhenAll(GetExecutionTasks(invocationList, eventItem, cancellationToken))
                    .ConfigureAwait(false);
            }
        }

        return eventItem;
    }

    private static IEnumerable<Task> GetExecutionTasks<TEvent>(
        Delegate[] invocationList,
        TEvent eventItem,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        foreach (var individualDelegate in invocationList)
        {
            if (individualDelegate is Func<TEvent, CancellationToken, Task> typedAsyncAction)
            {
                Task executionTask;

                try
                {
                    executionTask = typedAsyncAction(eventItem, cancellationToken);
                }
                catch (Exception caughtException)
                {
                    executionTask = Task.FromException(caughtException);
                }

                yield return executionTask;
            }
        }
    }
}
