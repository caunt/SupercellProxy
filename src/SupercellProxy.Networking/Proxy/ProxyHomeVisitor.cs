using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Turns;

namespace SupercellProxy.Networking.Proxy;

internal sealed class ProxyHomeVisitor(ProxyConnection connection)
{
    internal bool SuppressEndClientTurns { get; private set; }

    /// <summary>
    /// Executes the <c language="csharp">VisitHomeAsync</c> operation.
    /// </summary>
    internal async ValueTask<OtherHomeDataMessage> VisitHomeAsync(LongIdentifier target, CancellationToken cancellationToken = default)
    {
        SuppressEndClientTurns = true;

        try
        {
            await connection.WriteMessageAsync(new VisitHomeMessage { Unknown0 = 0x01, Unknown1 = 0x02 }, MessageDirection.Serverbound, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await connection.WriteMessageAsync(new VisitHomeTargetMessage { Unknown0 = 0x00, Target = target }, MessageDirection.Serverbound, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            OtherHomeDataMessage otherHomeDataMessage = await ExpectMessageAsync<OtherHomeDataMessage>(timeout: TimeSpan.FromSeconds(seconds: 15), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            do
            {
                try
                {
                    EndClientTurnMessage endClientTurnMessage = await ExpectMessageAsync<EndClientTurnMessage>(timeout: TimeSpan.FromSeconds(seconds: 3), cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    if (endClientTurnMessage.SubTick is 0)
                        break;
                }
                catch (TimeoutException)
                {
                    break;
                }
            } while (!cancellationToken.IsCancellationRequested);

            return otherHomeDataMessage;
        }
        finally
        {
            SuppressEndClientTurns = false;
        }
    }

    private static async Task CompleteExpectedMessageAsync<TMessage>(MessageSentEvent @event, TaskCompletionSource<TMessage> taskCompletionSource)
        where TMessage : class, IMessage
    {
        if (@event.Message is TMessage message && !taskCompletionSource.TrySetResult(message))
            return;

        await Task.CompletedTask.ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(TimeSpan timeout, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        using CancellationTokenSource linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        linkedCancellationTokenSource.CancelAfter(timeout);

        try
        {
            return await ExpectMessageAsync<TMessage>(linkedCancellationTokenSource.Token)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (TaskCanceledException)
        {
            throw new TimeoutException($"Expected message of type {typeof(TMessage)} was not received within the timeout period of {timeout}");
        }
    }

    private async Task<TMessage> ExpectMessageAsync<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        TaskCompletionSource<TMessage> taskCompletionSource = new();

        async Task Handler(MessageSentEvent @event, CancellationToken unusedParameter1)
        {
            await CompleteExpectedMessageAsync(@event, taskCompletionSource).ConfigureAwait(continueOnCapturedContext: false);
        }

        await connection.EventBus.SubscribeAsync((Func<MessageSentEvent, CancellationToken, Task>)Handler, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            return await taskCompletionSource
                .Task.WaitAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            await connection.EventBus.UnsubscribeAsync((Func<MessageSentEvent, CancellationToken, Task>)Handler, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

}
