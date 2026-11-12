using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>
/// <para>Buys back customizations at the renovator stand (native LogicBuyBackCustomizationsCommand).</para>
/// </summary>
public sealed record BuyBackCustomizationsCommand : CodecCommand<BuyBackCustomizationsCommand>, ICommandCodecProvider<BuyBackCustomizationsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BuyBackCustomizationsCommand"/> record. Describes one buy-back at the renovator stand.
    /// </summary>
    /// <param name="globalId">Id written first, before the list; see <see cref="GlobalId"/>.</param>
    /// <param name="globalIds">Global ids of the customization parts bought back.</param>
    /// <param name="unknown0">Number written after the list; see <see cref="Unknown0"/>.</param>
    /// <param name="diagnosticValues">Empty in production; otherwise one diagnostic number per part.</param>
    /// <exception cref="InvalidDataException">Diagnostic values were given but not one per part.</exception>
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
    public static CommandCodec<BuyBackCustomizationsCommand> Codec { get; } = Codecs.Command(Codecs.Create(static stream =>
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
            }),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets one number per bought-back part that only non-production builds append; empty in production.
    /// </summary>
    /// <value>Int32 values written after <see cref="Unknown0"/>, one per entry of <see cref="GlobalIds"/> outside production; writing a command whose count does not match throws <see cref="InvalidDataException"/>.</value>
    public ReadOnlyMemory<int> DiagnosticValues { get; }

    /// <summary>
    /// Gets the id written before the part list; which object it names (stand or category) is not identified.
    /// </summary>
    /// <value>The first VarInt of the command's fields, carried through unchanged.</value>
    [JsonPropertyName("GlobalId")]
    public int GlobalId { get; }

    /// <summary>
    /// Gets the global ids of the customization parts the player buys back.
    /// </summary>
    /// <value>A VarInt-counted list of part data global ids, each a part the player used before.</value>
    [JsonPropertyName("GlobalIds")]
    public ReadOnlyMemory<int> GlobalIds { get; }

    /// <summary>
    /// Gets the number written after the part list; its meaning is not identified.
    /// </summary>
    /// <value>The VarInt that follows <see cref="GlobalIds"/>, carried through unchanged.</value>
    public int Unknown0 { get; }

    private static ICodec<ReadOnlyMemory<int>> GlobalIdsCodec => field ??= Codecs.Memory(Codecs.VarInt);
}
