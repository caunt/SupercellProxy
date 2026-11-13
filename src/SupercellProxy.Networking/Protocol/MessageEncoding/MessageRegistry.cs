using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Versions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>Maps native message wire ids to message types and reads payloads with their codecs.</summary>
public static class MessageRegistry
{
    private static readonly FrozenDictionary<Type, MessageRegistryEntry> ByType = CreateEntries();

    /// <summary>Gets the wire id history of every message type.</summary>
    /// <value>
    /// A registry keyed by message record type, built once from every supported game release, oldest first; each release overrides only
    /// the ids it changes.
    /// </value>
    public static ProtocolIdRegistry<Type> Ids { get; } = new(GameReleases.All.Select(static release => new ReleaseIds<Type>(release.Version,
        Changes: [.. release.Messages.Select(static assignment => KeyValuePair.Create(assignment.Entry.Type, assignment.Id))])));

    /// <summary>Gets the name captures use for a message.</summary>
    /// <param name="message">A decoded message of a registered type, or a <see cref="PassthroughMessage"/>.</param>
    /// <returns>
    /// The capture name of the earliest release that registers the message's type, which is the type name unless that release gives an
    /// older name; <c>PassthroughMessage</c> for undecoded messages.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    public static string GetCaptureName(IMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        return message is PassthroughMessage ? nameof(PassthroughMessage) : GetEntry(message.GetType()).CaptureName;
    }

    /// <summary>Gets the wire id of a message in a release.</summary>
    /// <typeparam name="TValue">The static type of <paramref name="message"/>; the lookup uses the message's runtime type, so any <see cref="IMessage"/> works.</typeparam>
    /// <param name="message">The message whose id is wanted; a <see cref="PassthroughMessage"/> answers with the id it was received under.</param>
    /// <param name="gameVersion">The game release whose id table applies, or <see langword="null"/> to accept only an id every release agrees on.</param>
    /// <returns>The 16-bit wire id the release uses for the message's type.</returns>
    public static ushort GetId<TValue>(TValue message, Version? gameVersion)
        where TValue : IMessage
    {
        return message is PassthroughMessage passthroughMessage ? passthroughMessage.Id : GetId(message.GetType(), gameVersion);
    }

    /// <summary>Gets the wire id of a message type in a release.</summary>
    /// <typeparam name="TValue">The registered message record type, such as <c>LoginOkMessage</c>.</typeparam>
    /// <param name="gameVersion">The game release whose id table applies, or <see langword="null"/> to accept only an id every release agrees on.</param>
    /// <returns>The 16-bit wire id the release assigns to <typeparamref name="TValue"/>.</returns>
    public static ushort GetId<TValue>(Version? gameVersion)
        where TValue : IMessage
    {
        return GetId(typeof(TValue), gameVersion);
    }

    /// <summary>Gets the wire id of a message type in a release.</summary>
    /// <param name="type">The registered message record type.</param>
    /// <param name="gameVersion">
    /// The game release whose id table applies; a release without its own table uses the newest older one. With <see langword="null"/>, the
    /// type must have the same id in every release.
    /// </param>
    /// <returns>The 16-bit wire id the release assigns to <paramref name="type"/>.</returns>
    public static ushort GetId(Type type, Version? gameVersion)
    {
        return checked(ushort.CreateChecked(Ids.GetId(type, gameVersion)));
    }

    /// <summary>Gets a value indicating whether a wire id names a message type in a release.</summary>
    /// <typeparam name="TMessage">The message record type to test for.</typeparam>
    /// <param name="id">The wire id from a frame header.</param>
    /// <param name="gameVersion">The game release the frame belongs to, or <see langword="null"/> when it is not known yet, as before the login is accepted.</param>
    /// <returns>
    /// <see langword="true"/> if the release's id table assigns <paramref name="id"/> and gives it to <typeparamref name="TMessage"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool Is<TMessage>(ushort id, Version? gameVersion) where TMessage : IMessage
    {
        return Ids.Is(id, typeof(TMessage), gameVersion);
    }

    /// <summary>Reads a message payload with the codec of the type its id names, or keeps it raw when the id is unmodelled.</summary>
    /// <param name="container">
    /// A decrypted frame; the payload's game version picks the id table, and commands whose layout depends on game data are read with the
    /// stream's existing data resolver, if any.
    /// </param>
    /// <returns>The decoded message, or a <see cref="PassthroughMessage"/> holding the remaining payload bytes when no type owns the id.</returns>
    public static IMessage Resolve(MessageContainer container)
    {
        return Resolve(container, dataResolver: null);
    }

    /// <summary>Reads a message payload; commands inside it resolve polymorphic payloads through <paramref name="dataResolver"/>.</summary>
    /// <param name="container">A decrypted frame; the payload is read from its current position, and its game version picks the id table.</param>
    /// <param name="dataResolver">
    /// The game tables to install on the payload stream before decoding, or <see langword="null"/> to keep the resolver the stream already
    /// has.
    /// </param>
    /// <returns>The decoded message, or a <see cref="PassthroughMessage"/> holding the remaining payload bytes when no type owns the id.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null"/>.</exception>
    public static IMessage Resolve(MessageContainer container, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(container);

        MessageChannel payload = container.Payload;

        if (dataResolver is not null)
            payload.CommandDataResolver = dataResolver;

        return TryGetMessageType(container.Id, payload.GameVersion, out Type? type) ? ByType[type].Factory(payload) : PassthroughMessage.Decode(container);
    }

    /// <summary>Finds the message type a wire id names in a release.</summary>
    /// <param name="id">The wire id from a frame header.</param>
    /// <param name="gameVersion">The game release the frame belongs to, or <see langword="null"/> to accept only a type every release agrees on.</param>
    /// <param name="messageType">When this method returns <see langword="true"/>, the message record type registered for <paramref name="id"/>.</param>
    /// <returns><see langword="true"/> if the release assigns <paramref name="id"/> to a message type; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetMessageType(ushort id, Version? gameVersion, [MaybeNullWhen(returnValue: false)] out Type messageType)
    {
        return Ids.TryGetContract(id, gameVersion, out messageType);
    }

    // A message keeps the registration, and capture name, of the first release that assigns it.
    private static FrozenDictionary<Type, MessageRegistryEntry> CreateEntries()
    {
        Dictionary<Type, MessageRegistryEntry> entries = [];

        // Later releases are applied first so the earliest registration of each message wins.
        for (int index = GameReleases.All.Length - 1; index >= 0; index--)
        {
            foreach (MessageIdAssignment assignment in GameReleases.All[index].Messages)
                entries[assignment.Entry.Type] = assignment.Entry;
        }

        return entries.ToFrozenDictionary();
    }

    private static MessageRegistryEntry GetEntry(Type type)
    {
        return ByType.TryGetValue(type, out MessageRegistryEntry? entry)
            ? entry
            : throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
