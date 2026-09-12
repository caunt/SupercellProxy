using SupercellProxy.Networking.Protocol;

namespace SupercellProxy.Networking.Sessions;

internal static class ClientSessionValidation
{
    internal static void Validate(ClientSession session, string source)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (session.AccountIdentifier == LongIdentifier.Empty)
            throw new InvalidDataException($"Client session in {source} has an empty account ID.");

        if (session.AppStore == default || !Enum.IsDefined(session.AppStore))
            throw new InvalidDataException($"Client session in {source} has an invalid app store: {session.AppStore}.");

        if (string.IsNullOrWhiteSpace(session.FarmName))
            throw new InvalidDataException($"Client session in {source} has an empty farm name.");

        if (string.IsNullOrWhiteSpace(session.PassToken))
            throw new InvalidDataException($"Client session in {source} has an empty pass token.");

        if (session.SessionRefreshToken is not null && string.IsNullOrWhiteSpace(session.SessionRefreshToken))
            throw new InvalidDataException($"Client session in {source} has an empty Supercell ID refresh token.");

        if (session.SessionToken is { IsEmpty: true })
            throw new InvalidDataException($"Client session in {source} has an empty Supercell ID session token.");
    }
}
