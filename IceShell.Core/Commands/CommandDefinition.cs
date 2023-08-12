namespace IceShell.Core.Commands;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public class CommandDefinition
{
    internal Dictionary<char, ComplexOptionDefinition> Options { get; } = new();
    internal List<ComplexValueDefinition> Values { get; } = new();

    public bool VariableValues { get; set; }
    public bool GreedyString { get; set; }

    public PropertyInfo? VariableValueBuffer { get; set; }

    /// <summary>
    /// Creates a string that describes this command definition in a user-friendly way.
    /// </summary>
    /// <param name="cmdName">The name of the command to include in the usage.</param>
    /// <returns>The command usage definition string.</returns>
    public string GetUsage(string cmdName)
    {
        var builder = new StringBuilder();
        builder.Append(cmdName).Append(' ');

        Values.ForEach(x =>
        {
            if (x.Required)
            {
                builder.Append('<').Append(x.Name).Append('>');
            }
            else
            {
                builder.Append('[').Append(x.Name).Append(']');
            }

            builder.Append(' ');
        });

        if (Options.Any())
        {
            // TODO output all switches and options so it looks like DOS
            builder.Append("[options...]");
        }

        return builder.ToString();
    }

    public CommandDefinition Option(char name, bool hasValue, PropertyInfo property, bool required = false)
    {
        Option(new(name, hasValue, property, required));

        return this;
    }

    public CommandDefinition Option(ComplexOptionDefinition option)
    {
        Options.Add(option.ShortName, option);

        return this;
    }

    public CommandDefinition Value(ComplexValueDefinition definition, int position = -1)
    {
        if (Values.Any() && !Values.Last().Required && definition.Required)
        {
            throw new ArgumentException(ER.ComplexArgumentOrderFailure);
        }

        if (position > 0)
        {
            Values.EnsureCapacity(position);
            Values.Insert(position, definition);
        }
        else
        {
            Values.Add(definition);
        }

        return this;
    }

    /// <summary>
    /// Instructs the parsing routine that checks for required arguments will be done by the command rather than
    /// the parsing routine, and the parsing routine should only check options.
    /// </summary>
    /// <returns>The current <see cref="ComplexArgument"/> instance.</returns>
    public CommandDefinition VarValues()
    {
        VariableValues = true;
        return this;
    }

    /// <summary>
    /// Instructs the parsing routine that that the first variable will be a greedy string value, and all others
    /// will not be parsed.
    /// </summary>
    /// <returns>The current <see cref="ComplexArgument"/> instance.</returns>
    public CommandDefinition Greedy()
    {
        GreedyString = true;
        return this;
    }
}
