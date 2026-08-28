using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Transport;
using SupercellProxy.Playground.Social;

namespace SupercellProxy.Playground.Network.Connections.Client;

internal sealed partial class ScClient
{
    private HomeTurns? _homeTurns;
    private FieldPlots? _fieldPlots;
    private readonly Friends _friends = new();

    internal EndClientTurnMessage[] ApplyClientboundFrame(
        ushort id,
        ushort version,
        ReadOnlyMemory<byte> payload
    )
    {
        return ApplyClientbound(ResolveMessage(id, version, payload));
    }

    internal MessageContainer GenerateServerbound(
        ushort id,
        ushort version,
        ReadOnlyMemory<byte> inputPayload
    )
    {
        var inputMessage = ResolveMessage(id, version, inputPayload);

        if (inputMessage is not EndClientTurnMessage inputTurn)
            return inputMessage.ToContainer(id, version);

        var generatedTurn = HomeTurns.GenerateActionTurn(
            inputTurn.SubTick,
            inputTurn.Commands.Span
        );
        return generatedTurn.ToContainer(id, version);
    }

    private EndClientTurnMessage[] ApplyClientbound(IMessage message)
    {
        if (global::SupercellProxy.Playground.Home.HomeTurns.Handles(message))
            return HomeTurns.Apply(message);

        switch (message)
        {
            case ServerHelloMessage:
            case LoginFailedMessage:
            case LoginOkMessage:
            case KeepAliveOkMessage:
            case Clientbound20155Message:
                return [];
            case Clientbound26199Message friendMeta:
                _friends.Apply(friendMeta);
                return [];
            case OtherFishingHomeDataMessage:
            case OtherHomeDataMessage:
            case PassthroughMessage:
                throw CreateUnsupportedClientboundException(message);
            default:
                throw CreateUnsupportedClientboundException(message);
        }
    }

    private async Task ProcessClientboundAsync(
        IMessage message,
        CancellationToken cancellationToken
    )
    {
        foreach (var turn in ApplyClientbound(message))
        {
            await CurrentMessageStream
                .WriteMessageAsync(turn, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task EnsureHomeReadyAsync(CancellationToken cancellationToken)
    {
        if (_homeTurns?.IsReady is true)
            return;

        var login = await LoginAsync(cancellationToken).ConfigureAwait(false);
        var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
        stream.CommandDataResolver = new DataTableResolver(login.Resources);
        _homeTurns = new HomeTurns(TurnDataTableResolver);
        _fieldPlots = new FieldPlots(_homeTurns);
        HandleGoods(login.Resources);

        await stream
            .WriteMessageAsync(new KeepAliveMessage(), cancellationToken)
            .ConfigureAwait(false);

        while (!HomeTurns.IsReady)
        {
            var message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false);
            await ProcessClientboundAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task SendClientTurnAsync(
        EndClientTurnMessage turn,
        CancellationToken cancellationToken
    )
    {
        ValidateTurnRoundTrip(turn, CurrentMessageStream.CommandDataResolver);
        await CurrentMessageStream.WriteMessageAsync(turn, cancellationToken).ConfigureAwait(false);
        await AwaitTurnAcknowledgementAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task AwaitTurnAcknowledgementAsync(CancellationToken cancellationToken)
    {
        await CurrentMessageStream
            .WriteMessageAsync(new KeepAliveMessage(), cancellationToken)
            .ConfigureAwait(false);

        IMessage message;

        do
        {
            message = await CurrentMessageStream
                .ReadMessageAsync(cancellationToken)
                .ConfigureAwait(false);
            await ProcessClientboundAsync(message, cancellationToken).ConfigureAwait(false);
        } while (message is not KeepAliveOkMessage);
    }

    private IMessage ResolveMessage(ushort id, ushort version, ReadOnlyMemory<byte> payload)
    {
        using var payloadStream = MessageStream.Create(payload);
        var container = new MessageContainer(id, version, payloadStream);
        return CurrentMessageStream.ResolveMessage(container);
    }

    private static NotSupportedException CreateUnsupportedClientboundException(IMessage message)
    {
        var id = MessageRegistry.GetId(message);
        return new NotSupportedException(
            $"Clientbound message {id} ({message.GetType().Name}) is not implemented yet."
        );
    }

    private DataTableResolver TurnDataTableResolver =>
        CurrentMessageStream.CommandDataResolver as DataTableResolver
        ?? throw new InvalidOperationException("The client has no game data resolver.");

    private HomeTurns HomeTurns => _homeTurns ??= new HomeTurns(TurnDataTableResolver);

    private FieldPlots FieldPlots => _fieldPlots ??= new FieldPlots(HomeTurns);

    private MessageStream CurrentMessageStream =>
        _supercellStream
        ?? throw new InvalidOperationException("The client has no message stream.");
}
