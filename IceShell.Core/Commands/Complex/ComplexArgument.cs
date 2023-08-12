// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;

using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using IceShell.Core.Commands;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Provides parsing and definition service for complex command arguments.
/// </summary>
public class ComplexArgument
{
    public const char COMPLEX_OPTION_SYMBOL = '/';
    public const char COMPLEX_END_OF_OPTION_SYMBOL = '-';

    private readonly CommandParser _parser;
    private readonly CommandDefinition _definition;

    internal ComplexArgument(CommandParser parser, CommandDefinition definition)
    {
        _parser = parser;
        _definition = definition;
    }

    public ComplexArgumentParseResult Parse()
    {
        var endOfOptions = false;
        var beginEndOfOptions = false;
        var ignoreADash = false;

        var requiredArgCount = _definition.Values.Count(x => x.Required);

        var result = new ComplexArgumentParseResult();

        var valueNum = 0;

        while (_parser.CanRead())
        {
            var delimiter = _parser.Peek();

            // Step 1: Check end of options parser.
            if (delimiter == COMPLEX_END_OF_OPTION_SYMBOL
                && !endOfOptions)
            {
                System.Console.WriteLine("End of options symbol.");

                if (!beginEndOfOptions)
                {
                    beginEndOfOptions = true;
                    continue;
                }
                else
                {
                    endOfOptions = true;
                    beginEndOfOptions = false;
                    _ = _parser.ReadUnquotedString();
                    ignoreADash = true;
                    continue;
                }
            }

            // STEP 2: Is option symbol?
            if (delimiter == COMPLEX_OPTION_SYMBOL && !endOfOptions)
            {
                var opt = ParseOption(out var name);
                CheckOption(name, opt, out var definition);

                if (definition == null)
                {
                    throw new InvalidOperationException("Passed check, but no definition");
                }

                result.Option(definition, opt);
            }
            else
            {
                // Begin parsing non variable
                string? toAdd;

                if (_definition.GreedyString)
                {
                    toAdd = _parser.ReadToEnd();
                }
                else
                {
                    toAdd = _parser.ReadString();
                }

#if EXT_DEBUG_INFO
                System.Console.WriteLine("Decided to add value {0} (EOO {1}, DELIMITER {2})", toAdd, endOfOptions, delimiter);
#endif

                if (((toAdd == "-" || toAdd == "--") && (!endOfOptions || beginEndOfOptions || ignoreADash) // ignore dashes)
                    || string.IsNullOrWhiteSpace(toAdd)))
                {
                    continue;
                }

                // Variable values support
                if (_definition.VariableValues)
                {
                    result.VariableValues.Add(toAdd);
                    continue;
                }

                // Check value definition existence

                if (requiredArgCount != 0 && valueNum >= requiredArgCount)
                {
                    throw new CommandFormatException(Languages.ArgumentSurpassingCount(valueNum, _definition.Values.Count));
                }

                var def = _definition.Values[valueNum];
                result.Value(def, toAdd);

                valueNum++;
            }
        }

#if EXT_DEBUG_INFO
        values.ForEach(x => System.Console.WriteLine(x));
#endif

        if (!_definition.VariableValues && (requiredArgCount != 0 && valueNum <= requiredArgCount))
        {
            throw new CommandFormatException(Languages.ArgumentLowerThanCount(valueNum, requiredArgCount));
        }

        return result;
    }

    private void CheckOption(char name, string? value, out ComplexOptionDefinition? definition)
    {
        definition = null;

        if (!_definition.Options.TryGetValue(name, out var def))
        {
            definition = def;
            throw ExceptionHelper.WithName(ER.ComplexNonExistingOption, name);
        }

        if (!def.HasValue && value != null)
        {
            throw ExceptionHelper.WithName(ER.ComplexNoValueAllowed, name);
        }

        if (def.HasValue && value == null)
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, name);
        }
    }

    public bool OptionPresents(char name)
    {
        return _definition.Options.ContainsKey(name);
    }

    private string? ParseOption(out char name)
    {
        if (_parser.Read() != '/')
        {
            throw new CommandFormatException("Excepted delimiter");
        }

        name = _parser.Read();

        var delimiter = _parser.Peek();

        if (delimiter != ':')
        {
            return null;
        }

        return _parser.ReadString();
    }
}
