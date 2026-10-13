using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Releases;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>Maps native message wire ids to message types and reads payloads with their codecs.</summary>
public static class MessageRegistry
{
    private static readonly FrozenDictionary<Type, MessageRegistryEntry> ByType = CreateEntries();

    /// <summary>Gets the wire id history of every message type.</summary>
    public static ProtocolIdRegistry<Type> Ids { get; } = new(
        GameReleases.All.Select(
            static release => new ReleaseIds<Type>(release.Version, [.. release.Messages.Select(static assignment => KeyValuePair.Create(assignment.Entry.Type, assignment.Id))])
        )
    );

    /// <summary>Gets the name captures use for a message.</summary>
    public static string GetCaptureName(IMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        return message is PassthroughMessage ? nameof(PassthroughMessage) : GetEntry(message.GetType()).CaptureName;
    }

    /// <summary>Gets the wire id of a message in a release.</summary>
    public static ushort GetId<TValue>(TValue message, Version? gameVersion)
        where TValue : IMessage
    {
        return message is PassthroughMessage passthroughMessage ? passthroughMessage.Id : GetId(message.GetType(), gameVersion);
    }

    /// <summary>Gets the wire id of a message type in a release.</summary>
    public static ushort GetId<TValue>(Version? gameVersion)
        where TValue : IMessage
    {
        return GetId(typeof(TValue), gameVersion);
    }

    /// <summary>Gets the wire id of a message type in a release.</summary>
    public static ushort GetId(Type type, Version? gameVersion)
    {
        return checked((ushort)Ids.GetId(type, gameVersion));
    }

    /// <summary>Gets a value indicating whether a wire id names a message type in a release.</summary>
    public static bool Is<TMessage>(ushort id, Version? gameVersion) where TMessage : IMessage
    {
        return Ids.Is(id, typeof(TMessage), gameVersion);
    }

    /// <summary>Reads a message payload with the codec of the type its id names, or keeps it raw when the id is unmodelled.</summary>
    public static IMessage Resolve(MessageContainer container)
    {
        return Resolve(container, dataResolver: null);
    }

    /// <summary>Reads a message payload; commands inside it resolve polymorphic payloads through <paramref name="dataResolver"/>.</summary>
    public static IMessage Resolve(MessageContainer container, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream payload = container.Payload;

        if (dataResolver is not null)
            payload.CommandDataResolver = dataResolver;

        return TryGetMessageType(container.Id, payload.GameVersion, out Type? type) ? ByType[type].Factory(payload) : PassthroughMessage.Decode(container);
    }

    /// <summary>Finds the message type a wire id names in a release.</summary>
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
