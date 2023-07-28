// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;

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

    private readonly Dictionary<char, ComplexOptionDefinition> _optionDefinitions = new();
    private readonly List<ComplexValueDefinition> _valueDefinitions = new();

    internal ComplexArgument(CommandParser parser)
    {
        _parser = parser;
    }

    internal string GetUsage(string cmdName)
    {
        var builder = new StringBuilder();
        builder.Append(cmdName).Append(' ');

        _valueDefinitions.ForEach(x =>
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

        if (_optionDefinitions.Any())
        {
            // TODO output all switches and options so it looks like DOS
            builder.Append("[options...]");
        }

        return builder.ToString();
    }

    public void AddOption(char name, bool hasValue, bool required = false)
    {
        AddOption(new(name, hasValue, required));
    }

    public void AddOption(ComplexOptionDefinition option)
    {
        _optionDefinitions.Add(option.ShortName, option);
    }

    public void AddValue(string name, bool required = false)
    {
        AddValue(new(name, required));
    }

    public void AddValue(ComplexValueDefinition definition)
    {
        if (!_valueDefinitions.IsEmpty() && !_valueDefinitions.Last().Required && definition.Required)
        {
            throw new ArgumentException(ER.ComplexArgumentOrderFailure);
        }

        _valueDefinitions.Add(definition);
    }

    public ComplexArgumentParseResult Parse()
    {
        var options = new Dictionary<char, string?>();
        var values = new List<string?>();

        var endOfOptions = false;
        var beginEndOfOptions = false;
        var ignoreADash = false;

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
                CheckOption(name, opt);
                options.Add(name, opt);
            }
            else
            {
                var toAdd = _parser.ReadString();

#if EXT_DEBUG_INFO
                System.Console.WriteLine("Decided to add value {0} (EOO {1}, DELIMITER {2})", toAdd, endOfOptions, delimiter);
#endif

                if (((toAdd == "-" || toAdd == "--") && (!endOfOptions || beginEndOfOptions || ignoreADash) // ignore dashes)
                    || string.IsNullOrWhiteSpace(toAdd)))
                {
                    continue;
                }

                values.Add(toAdd);
            }
        }

#if EXT_DEBUG_INFO
        values.ForEach(x => System.Console.WriteLine(x));
#endif

        var liveCount = _valueDefinitions.Count(x => x.Required);

        if (values.Count < liveCount)
        {
            throw new CommandFormatException(string.Format(ER.MissingValues, _valueDefinitions.Count, values.Count));
        }

        foreach (var option in _optionDefinitions)
        {
            if (!options.ContainsKey(option.Key) && option.Value.Required)
            {
                throw ExceptionHelper.WithName(ER.ComplexMissingRequiredOption, option.Key);
            }
        }

        return new(options, values);
    }

    private void CheckOption(char name, string? value)
    {
        if (!_optionDefinitions.TryGetValue(name, out var def))
        {
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
        return _optionDefinitions.ContainsKey(name);
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
