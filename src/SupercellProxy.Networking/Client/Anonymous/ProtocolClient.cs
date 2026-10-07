using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Assets.Tables;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions.Anonymous;

namespace SupercellProxy.Networking.Client;

public sealed partial class ProtocolClient
{
    private static int? ProfileValue(DataTableResolver resolver, ClientAvatar avatar, string resource)
    {
        return resolver.TryResolve(GameAssetFiles.Money, resource, out DataTableReference? data)
            ? avatar.InventoryMaps[0].FirstOrDefault(item => item.GlobalDataId == data.GlobalId)?.Value
            : null;
    }

    private static bool RejectedCredentials(LoginException error)
    {
        return error.LoginFailedMessage?.ErrorCode is LoginFailureType.InvalidCredentials or LoginFailureType.InvalidToken;
    }

    private async Task<ClientLoginResult> AuthenticateAsync(bool anonymous, bool requestOwnHome, CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        using CancellationTokenSource request = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);

        using IDisposable gate = await _authenticationGate.LockAsync(request.Token).ConfigureAwait(continueOnCapturedContext: false);

        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        if (_login is not null) return _login;

        LogAuthenticating(_logger);

        try
        {
            _login = anonymous
                ? await LoginAnonymousAccountAsync(requestOwnHome, request.Token).ConfigureAwait(continueOnCapturedContext: false)
                : await _authenticator.LoginAsync(request.Token).ConfigureAwait(continueOnCapturedContext: false);

            if (_login.Resources.Length > 0) Stream.CommandDataResolver = new DataTableResolver(_login.Resources);

            return _login;
        }
        catch
        {
            await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    private async Task<ClientLoginResult> LoginAnonymousAccountAsync(bool requestOwnHome, CancellationToken cancellationToken)
    {
        // Bootstrap uses no account, and must finish before acquiring the account's lease.
        GameAssetBundle assets = await _authenticator.DiscoverAssetsAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (_anonymousAccounts is null)
        {
            return Configuration.AnonymousAccountId is not null
                ? throw new InvalidOperationException(message: "Saved anonymous accounts require the anonymous-account ledger.")
                : await _authenticator.LoginAnonymousAsync(assets, credentials: null, requestOwnHome, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AnonymousAccountLease lease = await _anonymousAccounts.AcquireAsync(Configuration.AnonymousAccountId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            _anonymousLease = lease;

            try
            {
                ClientLoginResult result = await _authenticator.LoginAnonymousAsync(assets, lease.Credentials, requestOwnHome, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

                LoginOkMessage login = result.LoginOkMessage;

                // A returned replacement pass token must survive cancellation immediately after login.
                using CancellationTokenSource saving = new(TimeSpan.FromSeconds(seconds: 10));

                await lease.SaveAsync(new(login.AccountId.AsInt64, login.PassToken), saving.Token).ConfigureAwait(continueOnCapturedContext: false);

                return result;
            }
            catch (LoginException error) when (RejectedCredentials(error) && lease.Credentials is not null)
            {
                using CancellationTokenSource saving = new(TimeSpan.FromSeconds(seconds: 10));

                await lease.RejectAsync(saving.Token).ConfigureAwait(continueOnCapturedContext: false);
                await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);

                if (Configuration.AnonymousAccountId is not null) throw;
            }
        }

        throw new OperationCanceledException(cancellationToken);
    }

    private void ObserveAnonymousProfile(IMessage? message)
    {
        AnonymousAccountLease? lease = _anonymousLease;

        if (lease?.Credentials is not { } credentials || _supercellStream?.CommandDataResolver is not DataTableResolver resolver) return;

        ClientAvatar? avatar = message switch
        {
            OwnHomeDataMessage home => home.ClientAvatar,
            OtherHomeDataMessage home => home.ClientAvatar,
            _ => null,
        };

        if (avatar is null || avatar.AccountId.AsInt64 != credentials.AccountId || avatar.InventoryMaps.Length == 0) return;

        int? level = ProfileValue(resolver, avatar, resource: "ExpLevel");
        int? experience = ProfileValue(resolver, avatar, resource: "ExpPoints");

        if (level is null) return;

        lease.Observe(
            new(avatar.FarmName, level.Value, experience ?? 0)
            {
                HomeId = avatar.HomeId.AsInt64,
                Coins = ProfileValue(resolver, avatar, resource: "Cash"),
                Diamonds = ProfileValue(resolver, avatar, resource: "Diamonds"),
            }
        );
    }

    private async ValueTask ObserveMessageAsync(MessageDirection direction, MessageContainer container, IMessage? message)
    {
        if (direction == MessageDirection.Clientbound) ObserveAnonymousProfile(message);

        if (Configuration.ObserveMessage is { } observer) await observer(direction, container, message).ConfigureAwait(continueOnCapturedContext: false);
    }
}
