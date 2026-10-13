using System.Collections.Frozen;
using System.Globalization;

using SupercellProxy.Networking.Protocol.Versions;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>Maps native command wire ids to command contracts and reads and writes commands with their codecs.</summary>
public static class CommandRegistry
{
    private static readonly FrozenDictionary<int, CommandContract> Contracts = CreateContracts();

    /// <summary>Gets the wire id history of every native command contract.</summary>
    public static ProtocolIdRegistry<int> Ids { get; } = new(
        GameReleases.All.Select(
            static release => new ReleaseIds<int>(release.Version, [.. release.Commands.Select(static assignment => KeyValuePair.Create(assignment.Contract.Type, assignment.Id))])
        )
    );

    /// <summary>
    /// Gets the codec of a command preceded by its wire id, using the stream's command environment and data resolver.
    /// </summary>
    public static ICodec<Command> Codec { get; } = Codecs.Create(Decode, Encode);

    /// <summary>Gets every modelled command contract by its internal id.</summary>
    public static IReadOnlyDictionary<int, CommandContract> Registrations => Contracts;

    /// <summary>Reads one command, wire id first, in the stream's game version and command environment.</summary>
    public static Command Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return Decode(stream.ReadVarInt(), stream);
    }

    /// <summary>Reads one command whose wire id has already been read.</summary>
    public static Command Decode(int id, MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        CommandContract contract = TryGetCommandType(id, stream.GameVersion, out int commandType) && Contracts.TryGetValue(commandType, out CommandContract? found)
            ? found
            : throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command id {id} is not supported in game version {stream.GameVersion}."));

        EnsureAllowedEnvironment(contract, stream.CommandEnvironment);

        try
        {
            return contract.ReadCommand(stream);
        }
        catch (InvalidDataException exception)
        {
            throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Command {commandType} decoding failed at byte {stream.Position}: {exception.Message}"),
                exception
            );
        }
    }

    /// <summary>Writes one command, wire id first, in the stream's game version and command environment.</summary>
    public static void Encode(ProtocolEncoder encoder, Command command)
    {
        ArgumentNullException.ThrowIfNull(encoder);
        ArgumentNullException.ThrowIfNull(command);

        if (!Contracts.TryGetValue(command.Type, out CommandContract? contract) || !contract.CommandType.IsInstanceOfType(command))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {command.Type} is not supported."));

        EnsureAllowedEnvironment(contract, encoder.CommandEnvironment);
        encoder.WriteVarInt(GetId(command, encoder.GameVersion));
        contract.WriteCommand(encoder, command);
    }

    /// <summary>Gets the wire id of a command in a release.</summary>
    public static int GetId(Command command, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(command);

        return Ids.GetId(command.Type, gameVersion);
    }

    /// <summary>Gets the wire id of a command contract in a release.</summary>
    public static int GetId(int commandType, Version? gameVersion)
    {
        return Ids.GetId(commandType, gameVersion);
    }

    /// <summary>Gets a value indicating whether a wire id names a command contract in a release.</summary>
    public static bool Is(int id, int commandType, Version? gameVersion)
    {
        return Ids.Is(id, commandType, gameVersion);
    }

    /// <summary>Finds the command contract a wire id names in a release.</summary>
    public static bool TryGetCommandType(int id, Version? gameVersion, out int commandType)
    {
        return Ids.TryGetContract(id, gameVersion, out commandType);
    }

    private static FrozenDictionary<int, CommandContract> CreateContracts()
    {
        Dictionary<int, CommandContract> contracts = [];

        foreach (GameRelease release in GameReleases.All)
        {
            foreach (CommandIdAssignment assignment in release.Commands)
                contracts[assignment.Contract.Type] = assignment.Contract;
        }

        return contracts.ToFrozenDictionary();
    }

    private static void EnsureAllowedEnvironment(CommandContract contract, CommandEnvironment environment)
    {
        if (environment is CommandEnvironment.Production && !contract.ProductionAllowed)
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {contract.Type} is not allowed in the production environment."));
    }
}
