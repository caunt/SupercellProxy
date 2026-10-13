using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>
/// <para>Buys back customizations at the renovator stand (native LogicBuyBackCustomizationsCommand).</para>
/// </summary>
public sealed record BuyBackCustomizationsCommand : CodecCommand<BuyBackCustomizationsCommand>, ICommandCodecProvider<BuyBackCustomizationsCommand>
{
    /// <summary>
    /// Initializes a new <see cref="BuyBackCustomizationsCommand"/> instance.
    /// </summary>
    public BuyBackCustomizationsCommand(int globalId, ReadOnlyMemory<int> globalIds, int unknown0, ReadOnlyMemory<int> diagnosticValues)
    {
        if (diagnosticValues.Length is not 0 && diagnosticValues.Length != globalIds.Length)
            throw new InvalidDataException(message: "The buy-back customizations command diagnostic values must be empty or match the data-reference count.");

        GlobalId = globalId;
        GlobalIds = globalIds.ToArray();
        Unknown0 = unknown0;
        DiagnosticValues = diagnosticValues.ToArray();
    }

    /// <inheritdoc />
    /// <remarks>Outside production native appends one Int32 diagnostic value per customization.</remarks>
    public static CommandCodec<BuyBackCustomizationsCommand> Codec { get; } = Codecs.Command(
        Codecs.Create(
            static stream =>
            {
                int globalId = stream.ReadVarInt();
                ReadOnlyMemory<int> globalIds = GlobalIdsCodec.Read(stream);
                int unknown0 = stream.ReadVarInt();
                int[] diagnosticValues = stream.CommandEnvironment is CommandEnvironment.Production ? [] : new int[globalIds.Length];

                for (int index = 0; index < diagnosticValues.Length; index++)
                    diagnosticValues[index] = stream.ReadInt32();

                return new BuyBackCustomizationsCommand(globalId, globalIds, unknown0, diagnosticValues);
            },
            static (stream, command) =>
            {
                bool production = stream.CommandEnvironment is CommandEnvironment.Production;

                if (!production && command.DiagnosticValues.Length != command.GlobalIds.Length)
                    throw new InvalidDataException(message: "The buy-back customizations command requires one diagnostic value per data reference outside production.");

                stream.WriteVarInt(command.GlobalId);
                GlobalIdsCodec.Write(stream, command.GlobalIds);
                stream.WriteVarInt(command.Unknown0);

                if (production)
                    return;

                foreach (int diagnosticValue in command.DiagnosticValues.Span)
                    stream.WriteInt32(diagnosticValue);
            }
        ),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the <c language="csharp">DiagnosticValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> DiagnosticValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">GlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GlobalId")]
    public int GlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">GlobalIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GlobalIds")]
    public ReadOnlyMemory<int> GlobalIds { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    private static ICodec<ReadOnlyMemory<int>> GlobalIdsCodec => field ??= Codecs.Memory(Codecs.VarInt);
}
