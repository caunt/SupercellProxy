using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using SupercellProxy.Networking.Protocol.Versions;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Commands.Registry;

/// <summary>Maps native command wire ids to command contracts and reads and writes commands with their codecs.</summary>
public static class CommandRegistry
{
    /// <summary>Gets every modelled command contract.</summary>
    /// <value>Each contract that at least one release assigns a wire id, including contracts later releases removed.</value>
    public static ImmutableArray<CommandContract> Contracts => Ids.Contracts;

    /// <summary>Gets the wire id history of every native command contract.</summary>
    /// <value>A registry built from <see cref="GameReleases.All"/>, oldest release first, in which each release overrides only the command ids it changes.</value>
    public static ProtocolIdRegistry<CommandContract> Ids { get; } = new(GameReleases.All.Select(static release => new ReleaseIds<CommandContract>(release.Version,
        Changes: [.. release.Commands.Select(static assignment => KeyValuePair.Create(assignment.Contract, assignment.Id))])));

    /// <summary>
    /// Gets the codec of a command preceded by its wire id, using the stream's command environment and data resolver.
    /// </summary>
    /// <value>A codec whose read is <see cref="Decode(MessageChannel)"/> and whose write is <see cref="Encode"/>, for embedding commands in message payloads.</value>
    public static ICodec<Command> Codec { get; } = Codecs.Create(Decode, Encode);

    /// <summary>Reads one command, wire id first, in the stream's game version and command environment.</summary>
    /// <param name="stream">The payload positioned at the command's VarInt wire id; it is advanced past the whole command.</param>
    /// <returns>The decoded command record with its metadata applied; the concrete type is the record of the contract the id names.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    public static Command Decode(MessageChannel stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return Decode(stream.ReadVarInt(), stream);
    }

    /// <summary>Reads one command whose wire id has already been read.</summary>
    /// <param name="id">The command's wire id, interpreted in <see cref="ProtocolEncoder.GameVersion"/> of <paramref name="stream"/>.</param>
    /// <param name="stream">The payload positioned just after the wire id; it is advanced past the command's metadata and fields.</param>
    /// <returns>The decoded command record of the contract <paramref name="id"/> names, with its metadata applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    /// <exception cref="NotSupportedException"><paramref name="id"/> names no command in the stream's game version, or the command is not allowed in the stream's production environment.</exception>
    /// <exception cref="InvalidDataException">The command's bytes are malformed; the message names the contract, the id and the byte position where decoding failed, and the original error is the inner exception.</exception>
    public static Command Decode(int id, MessageChannel stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        CommandContract contract = TryGetContract(id, stream.GameVersion, out CommandContract? found)
            ? found
            : throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command id {id} is not supported in game version {stream.GameVersion}."));

        EnsureAllowedEnvironment(contract, stream.CommandEnvironment);

        try
        {
            return contract.ReadCommand(stream);
        }
        catch (InvalidDataException exception)
        {
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Command {contract} (id {id}) decoding failed at byte {stream.Position}: {exception.Message}"),
                exception);
        }
    }

    /// <summary>Writes one command, wire id first, in the stream's game version and command environment.</summary>
    /// <param name="encoder">The encoder to append to; its game version picks the wire id and its command environment decides whether debug data is written.</param>
    /// <param name="command">The command to write with its metadata, through the codec of its own <see cref="Command.Contract"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="encoder"/> or <paramref name="command"/> is <see langword="null"/>.</exception>
    /// <exception cref="NotSupportedException">The command's contract does not read and write the command's runtime type, the contract is not allowed in the encoder's production environment, or the encoder's game version assigns the contract no wire id.</exception>
    public static void Encode(ProtocolEncoder encoder, Command command)
    {
        ArgumentNullException.ThrowIfNull(encoder);
        ArgumentNullException.ThrowIfNull(command);

        CommandContract contract = command.Contract;

        if (!contract.CommandType.IsInstanceOfType(command))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command {contract} does not read and write {command.GetType().Name}."));

        EnsureAllowedEnvironment(contract, encoder.CommandEnvironment);
        encoder.WriteVarInt(GetId(contract, encoder.GameVersion));
        contract.WriteCommand(encoder, command);
    }

    /// <summary>Gets the wire id of a command in a release.</summary>
    /// <param name="command">The command whose <see cref="Command.Contract"/> is looked up.</param>
    /// <param name="gameVersion">The client's game release, where the newest release at or before it decides; <see langword="null"/> requires every release that knows the contract to agree on its id.</param>
    /// <returns>The wire id the release assigns to the command's contract.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="command"/> is <see langword="null"/>.</exception>
    public static int GetId(Command command, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(command);

        return GetId(command.Contract, gameVersion);
    }

    /// <summary>Gets the wire id of a command contract in a release.</summary>
    /// <param name="contract">The command contract to look up, usually a command record's codec.</param>
    /// <param name="gameVersion">The client's game release, where the newest release at or before it decides; <see langword="null"/> requires every release that knows <paramref name="contract"/> to agree on its id.</param>
    /// <returns>The wire id the release assigns to <paramref name="contract"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/>.</exception>
    public static int GetId(CommandContract contract, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return Ids.GetId(contract, gameVersion);
    }

    /// <summary>Gets a value indicating whether a wire id names a command contract in a release.</summary>
    /// <param name="id">The wire id read from a command payload.</param>
    /// <param name="contract">The contract to compare the id's owner with.</param>
    /// <param name="gameVersion">The client's game release, or <see langword="null"/> to require that every release using <paramref name="id"/> gives it to the same contract.</param>
    /// <returns><see langword="true"/> if <paramref name="id"/> is assigned in the release and resolves to <paramref name="contract"/> there; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/>.</exception>
    public static bool Is(int id, CommandContract contract, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return Ids.Is(id, contract, gameVersion);
    }

    /// <summary>Finds the command contract a wire id names in a release.</summary>
    /// <param name="id">The wire id read from a command payload.</param>
    /// <param name="gameVersion">The client's game release, where the newest release at or before it decides and an older version finds nothing; <see langword="null"/> resolves only an id every release gives to the same contract.</param>
    /// <param name="contract">When this method returns <see langword="true"/>, the contract that owns <paramref name="id"/>; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a contract owns <paramref name="id"/> in the release; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetContract(int id, Version? gameVersion, [MaybeNullWhen(returnValue: false)] out CommandContract contract)
    {
        return Ids.TryGetContract(id, gameVersion, out contract);
    }

    private static void EnsureAllowedEnvironment(CommandContract contract, CommandEnvironment environment)
    {
        if (environment is CommandEnvironment.Production && !contract.ProductionAllowed)
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command {contract} is not allowed in the production environment."));
    }
}
