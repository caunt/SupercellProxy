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
    /// <summary>Gets the wire id history of every native command contract.</summary>
    public static ProtocolIdRegistry<CommandContract> Ids { get; } = new(
        GameReleases.All.Select(
            static release => new ReleaseIds<CommandContract>(release.Version, [.. release.Commands.Select(static assignment => KeyValuePair.Create(assignment.Contract, assignment.Id))])
        )
    );

    /// <summary>Gets every modelled command contract.</summary>
    public static ImmutableArray<CommandContract> Contracts => Ids.Contracts;

    /// <summary>
    /// Gets the codec of a command preceded by its wire id, using the stream's command environment and data resolver.
    /// </summary>
    public static ICodec<Command> Codec { get; } = Codecs.Create(Decode, Encode);

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
            throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Command {contract} (id {id}) decoding failed at byte {stream.Position}: {exception.Message}"),
                exception
            );
        }
    }

    /// <summary>Writes one command, wire id first, in the stream's game version and command environment.</summary>
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
    public static int GetId(Command command, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(command);

        return GetId(command.Contract, gameVersion);
    }

    /// <summary>Gets the wire id of a command contract in a release.</summary>
    public static int GetId(CommandContract contract, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return Ids.GetId(contract, gameVersion);
    }

    /// <summary>Gets a value indicating whether a wire id names a command contract in a release.</summary>
    public static bool Is(int id, CommandContract contract, Version? gameVersion)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return Ids.Is(id, contract, gameVersion);
    }

    /// <summary>Finds the command contract a wire id names in a release.</summary>
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
