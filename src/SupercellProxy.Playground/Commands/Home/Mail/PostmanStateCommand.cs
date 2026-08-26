using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Applies the native command-694 postman state transition.</para>
/// </summary>
public sealed record PostmanStateCommand : Command
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 694;

    /// <summary>
    /// Defines the <c>RequiredState</c> value.
    /// </summary>
    public const int RequiredState = 11;

    /// <summary>
    /// Defines the <c>ResultState</c> value.
    /// </summary>
    public const int ResultState = 2;

    /// <summary>
    /// Initializes a new <see cref="PostmanStateCommand"/> instance.
    /// </summary>
    public PostmanStateCommand(
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executeSubTick, debugData0, debugData1) { }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    internal static PostmanStateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        var fields = DecodeCommand(stream, environment);
        return new PostmanStateCommand(fields.ExecuteSubTick, fields.DebugData0, fields.DebugData1);
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
    }
}
