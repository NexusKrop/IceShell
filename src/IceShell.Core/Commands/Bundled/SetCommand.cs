namespace IceShell.Core.Commands.Bundled;

using IceShell.Core.Commands.Attributes;
using IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.IO;

/// <summary>
/// Displays or modifies an environment variable.
/// </summary>
[ComplexCommand("SET", "Displays or modifies environment variable.")]
[GreedyString]
public class SetCommand : ICommand
{
    /// <summary>
    /// The name of the variable.
    /// </summary>
    [Value("variable", position: 0)]
    public string? VariableName { get; set; }

    /// <summary>
    /// The value of the variable. If not specified, displays the current value.
    /// </summary>
    [Value("value", position: 1, required: false)]
    public string? VariableValue { get; set; }

    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

        if (string.IsNullOrWhiteSpace(VariableName))
        {
            throw ExceptionHelper.RequiresValue(0);
        }

        if (VariableValue is null)
        {
            var envVar = Environment.GetEnvironmentVariable(VariableName);

            pipeStream = envVar == null ? TextReader.Null : new StringReader(envVar);

            return 0;
        }

        Environment.SetEnvironmentVariable(VariableName, VariableValue);

        return 0;
    }
}
