using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Versions;

/// <summary>
/// <para>One game release and the wire ids it assigns.</para>
/// <para>Each release lists only the ids that differ from the release before it; every other id carries over unchanged.</para>
/// </summary>
/// <param name="Version">The release version.</param>
/// <param name="Commands">The command ids this release adds, changes or removes.</param>
/// <param name="Messages">The message ids this release adds, changes or removes.</param>
internal sealed record GameRelease(Version Version, ImmutableArray<CommandIdAssignment> Commands, ImmutableArray<MessageIdAssignment> Messages)
{
    /// <summary>
    /// Assigns a command contract its wire id from this release onward.
    /// </summary>
    internal static CommandIdAssignment Command(CommandContract contract, int id)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return new CommandIdAssignment(contract, id);
    }

    /// <summary>
    /// Assigns a message type its wire id from this release onward, optionally with the name its capture files use.
    /// </summary>
    internal static MessageIdAssignment Message<TMessage>(int id, string? captureName = null)
        where TMessage : class, IMessage, ICodecProvider<TMessage>
    {
        MessageRegistryEntry entry = MessageRegistryEntry.Create<TMessage>();

        return new MessageIdAssignment(captureName is null ? entry : entry with { CaptureName = captureName }, id);
    }

    /// <summary>
    /// Removes a command contract from this release onward.
    /// </summary>
    internal static CommandIdAssignment RemovedCommand(CommandContract contract)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return new CommandIdAssignment(contract, Id: null);
    }

    /// <summary>
    /// Removes a message type from this release onward.
    /// </summary>
    internal static MessageIdAssignment RemovedMessage<TMessage>()
        where TMessage : class, IMessage, ICodecProvider<TMessage>
    {
        return new MessageIdAssignment(MessageRegistryEntry.Create<TMessage>(), Id: null);
    }
}
