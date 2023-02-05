// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides parsing and definition service for complex command arguments.
/// </summary>
public class ComplexArgument
{
    public const char COMPLEX_OPTION_SYMBOL = '/';

    private readonly CommandParser _parser;

    private readonly Dictionary<char, ComplexOptionDefinition> _optionDefinitions = new();
    private readonly List<ComplexValueDefinition> _valueDefinitions = new();

    internal ComplexArgument(CommandParser parser)
    {
        _parser = parser;
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

        var optionsBegun = false;

        while (_parser.CanRead())
        {
            var delimiter = _parser.Peek(1);

            if (delimiter == COMPLEX_OPTION_SYMBOL)
            {
                optionsBegun = true;

                var opt = ParseOption(out var name);
                CheckOption(name, opt);
                options.Add(name, opt);
            }
            else
            {
                if (optionsBegun)
                {
                    throw new CommandFormatException(ER.ComplexPreceedingOption);
                }

                values.Add(_parser.ReadString());
            }
        }

        if (values.Count < _valueDefinitions.Count)
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
        name = _parser.Read();

        var delimiter = _parser.Read();

        if (delimiter != ':')
        {
            return null;
        }

        return _parser.ReadString();
    }
}
