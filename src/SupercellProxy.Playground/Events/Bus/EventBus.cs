using Nito.AsyncEx;

namespace SupercellProxy.Playground.Events.Bus;

/// <summary>
/// Represents <c language="csharp">EventBus</c>.
/// </summary>
internal sealed class EventBus
{
    private Delegate?[] _eventDelegates = new Delegate?[16];
    private readonly AsyncLock _subscriptionLock = new();

    /// <summary>
    /// Executes the <c language="csharp">SubscribeAsync</c> operation.
    /// </summary>
    public async Task SubscribeAsync<TEvent>(
        Func<TEvent, CancellationToken, Task> asyncEventHandler,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        var eventIndex = EventTypeCache<TEvent>.Index;

        using var disposable = await _subscriptionLock
            .LockAsync(cancellationToken)
            .ConfigureAwait(false);

        var currentDelegatesArray = Volatile.Read(ref _eventDelegates);

        if (eventIndex >= currentDelegatesArray.Length)
        {
            var newArraySize = Math.Max(currentDelegatesArray.Length * 2, eventIndex + 1);
            var newDelegatesArray = new Delegate?[newArraySize];
            Array.Copy(currentDelegatesArray, newDelegatesArray, currentDelegatesArray.Length);

            newDelegatesArray[eventIndex] = asyncEventHandler;

            Volatile.Write(ref _eventDelegates, newDelegatesArray);
        }
        else
        {
            var existingDelegate = Volatile.Read(ref currentDelegatesArray[eventIndex]);
            var updatedDelegate = Delegate.Combine(existingDelegate, asyncEventHandler);

            Volatile.Write(ref currentDelegatesArray[eventIndex], updatedDelegate);
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">UnsubscribeAsync</c> operation.
    /// </summary>
    public async Task UnsubscribeAsync<TEvent>(
        Func<TEvent, CancellationToken, Task> asyncEventHandler,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        var eventIndex = EventTypeCache<TEvent>.Index;

        using var disposable = await _subscriptionLock
            .LockAsync(cancellationToken)
            .ConfigureAwait(false);

        var currentDelegatesArray = Volatile.Read(ref _eventDelegates);

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
    /// Executes the <c language="csharp">PublishAsync</c> operation.
    /// </summary>
    public async Task<TEvent> PublishAsync<TEvent>(
        TEvent eventItem,
        CancellationToken cancellationToken = default
    )
        where TEvent : IEvent
    {
        var eventIndex = EventTypeCache<TEvent>.Index;
        var currentDelegatesArray = Volatile.Read(ref _eventDelegates);

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
                yield return InvokeAsync(typedAsyncAction, eventItem, cancellationToken);
            }
        }
    }

    private static async Task InvokeAsync<TEvent>(
        Func<TEvent, CancellationToken, Task> asyncEventHandler,
        TEvent eventItem,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask.ConfigureAwait(false);
        await asyncEventHandler(eventItem, cancellationToken).ConfigureAwait(false);
    }
}
