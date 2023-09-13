namespace IceShell.Core.Commands;

/// <summary>
/// Represents context for command execution events, either for a single triggering,
/// a single shell, or a single batch file.
/// </summary>
public record ExecutionContext
{
    public ExecutionContext(TextWriter? writeTo, TextReader? readFrom)
    {
        ResultTo = writeTo;
        Retrieval = readFrom;
    }

    /// <summary>
    /// Gets the text writer to write the result of execution to.
    /// </summary>
    /// <remarks>
    /// You should not write error messages and information outputs to this text writer.
    /// </remarks>
    public TextWriter? ResultTo { get; }

    /// <summary>
    /// Gets the text reader to read the result of the last command from.
    /// </summary>
    public TextReader? Retrieval { get; }
}
