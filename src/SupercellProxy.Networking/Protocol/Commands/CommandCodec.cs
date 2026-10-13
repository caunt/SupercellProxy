using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>Reads and writes one command type: its fields and the shared metadata in native order.</summary>
/// <typeparam name="TCommand">The command record.</typeparam>
public sealed class CommandCodec<TCommand> : CommandContract, ICodec<TCommand> where TCommand : Command
{
    private static readonly bool IsServerCommand = typeof(ServerCommand).IsAssignableFrom(typeof(TCommand));
    private readonly ICodec<TCommand> _fields;

    internal CommandCodec(int type, ICodec<TCommand> fields, CommandLayout layout, bool productionAllowed) : base(type, layout, productionAllowed)
    {
        _fields = fields;
    }

    /// <inheritdoc />
    public override Type CommandType => typeof(TCommand);

    /// <inheritdoc />
    public override MessageDirection Direction => IsServerCommand ? MessageDirection.Clientbound : MessageDirection.Serverbound;

    private static ICodec<CommandMetadata> MetadataCodec => IsServerCommand ? CommandMetadata.ServerCodec : CommandMetadata.ClientCodec;


    /// <inheritdoc />
    public TCommand Read(MessageStream stream)
    {
        CommandMetadata? leading = Layout is CommandLayout.MetadataFirst ? MetadataCodec.Read(stream) : null;
        TCommand command = _fields.Read(stream);
        CommandMetadata metadata = leading ?? MetadataCodec.Read(stream);
        metadata.Apply(command);

        return command;
    }

    /// <inheritdoc />
    public void Write(ProtocolEncoder encoder, TCommand value)
    {
        ArgumentNullException.ThrowIfNull(value);

        CommandMetadata metadata = CommandMetadata.Of(value);

        if (Layout is CommandLayout.MetadataFirst)
            MetadataCodec.Write(encoder, metadata);

        _fields.Write(encoder, value);

        if (Layout is CommandLayout.FieldsFirst)
            MetadataCodec.Write(encoder, metadata);
    }

    internal override Command ReadCommand(MessageStream stream)
    {
        return Read(stream);
    }

    internal override void WriteCommand(ProtocolEncoder encoder, Command command)
    {
        Write(encoder, command as TCommand ?? throw new InvalidDataException($"Expected a {typeof(TCommand).Name}."));
    }
}
