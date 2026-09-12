using Nito.AsyncEx;

namespace SupercellProxy.Networking.Events;

/// <summary>
/// Represents <c language="csharp">EventBus</c>.
/// </summary>
public sealed class EventBus
{
    private Delegate?[] _eventDelegates = new Delegate?[16];
    private readonly AsyncLock _subscriptionLock = new();

    /// <summary>
    /// Executes the <c language="csharp">PublishAsync</c> operation.
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent eventItem, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        int eventIndex = EventTypeCache<TEvent>.Index;
        Delegate?[] currentDelegatesArray = Volatile.Read(ref _eventDelegates);

        if (eventIndex < currentDelegatesArray.Length)
        {
            Delegate? currentDelegates = Volatile.Read(ref currentDelegatesArray[eventIndex]);

            if (currentDelegates is not null)
            {
                Delegate[] invocationList = currentDelegates.GetInvocationList();
                await Task.WhenAll(GetExecutionTasks(invocationList, eventItem, cancellationToken))
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

    }

    /// <summary>
    /// Executes the <c language="csharp">SubscribeAsync</c> operation.
    /// </summary>
    public async Task SubscribeAsync<TEvent>(Func<TEvent, CancellationToken, Task> asynchronouslyEventHandler, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        int eventIndex = EventTypeCache<TEvent>.Index;

        using IDisposable disposable = await _subscriptionLock
            .LockAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        Delegate?[] currentDelegatesArray = Volatile.Read(ref _eventDelegates);

        if (eventIndex >= currentDelegatesArray.Length)
        {
            int newArraySize = Math.Max(currentDelegatesArray.Length * 2, eventIndex + 1);
            Delegate?[] newDelegatesArray = new Delegate?[newArraySize];
            Array.Copy(currentDelegatesArray, newDelegatesArray, currentDelegatesArray.Length);

            newDelegatesArray[eventIndex] = asynchronouslyEventHandler;

            Volatile.Write(ref _eventDelegates, newDelegatesArray);
        }
        else
        {
            Delegate? existingDelegate = Volatile.Read(ref currentDelegatesArray[eventIndex]);
            Delegate updatedDelegate = Delegate.Combine(existingDelegate, asynchronouslyEventHandler);

            Volatile.Write(ref currentDelegatesArray[eventIndex], updatedDelegate);
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">UnsubscribeAsync</c> operation.
    /// </summary>
    public async Task UnsubscribeAsync<TEvent>(Func<TEvent, CancellationToken, Task> asynchronouslyEventHandler, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        int eventIndex = EventTypeCache<TEvent>.Index;

        using IDisposable disposable = await _subscriptionLock
            .LockAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        Delegate?[] currentDelegatesArray = Volatile.Read(ref _eventDelegates);

        if (eventIndex < currentDelegatesArray.Length)
        {
            Delegate? existingDelegate = Volatile.Read(ref currentDelegatesArray[eventIndex]);

            if (existingDelegate is not null)
            {
                Delegate? updatedDelegate = Delegate.Remove(existingDelegate, asynchronouslyEventHandler);
                Volatile.Write(ref currentDelegatesArray[eventIndex], updatedDelegate);
            }
        }
    }

    private static IEnumerable<Task> GetExecutionTasks<TEvent>(Delegate[] invocationList, TEvent eventItem, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        foreach (Delegate individualDelegate in invocationList)
        {
            if (individualDelegate is Func<TEvent, CancellationToken, Task> typedAsynchronouslyAction)
                yield return InvokeAsync(typedAsynchronouslyAction, eventItem, cancellationToken);
        }
    }

    private static async Task InvokeAsync<TEvent>(Func<TEvent, CancellationToken, Task> asynchronouslyEventHandler, TEvent eventItem, CancellationToken cancellationToken)
    {
        await Task.CompletedTask.ConfigureAwait(continueOnCapturedContext: false);
        await asynchronouslyEventHandler(eventItem, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
