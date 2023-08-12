// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Defines a command that lists directories and files inside the current or otherwise specified
/// directory.
/// </summary>
[ComplexCommand("dir", "Displays a list of files and subdirectories in a directory.")]
internal class DirCommandEx : IComplexCommand
{
    private sealed record class DirTableRow(string ShortDateTime, string Size, string FileName);

    private readonly List<DirTableRow> _rows = new();

    private int _fileCount;
    private int _dirCount;

    private int _dateLongest;
    private int _sizeLongest;
    private int _fileNameLongest;

    private string _dateFormat = "yyyy/MM/dd";
    private string _timeFormat = "HH:mm";
    private string _dir = Environment.CurrentDirectory;

    private string GetTableDateTime(DateTime time)
    {
        // Use a string builder
        // As we need to dynamically determine the space between sdStr and stStr
        var builder = new StringBuilder();

        // sdStr = short date string
        var sdStr = time.ToString(_dateFormat);

        // stStr = short time string
        var stStr = time.ToString(_timeFormat);

        // We add short date string first.
        builder.Append(sdStr);

        // This apply to zh-CN culture
        // Not sure about others
        if (sdStr.Length == 9 && stStr.Length == 5)
        {
            // Use a single space
            builder.Append(' ');
        }
        else
        {
            // Use two spaces
            builder.Append("  ");
        }

        // Append the short time string
        builder.Append(stStr);

        // Return
        return builder.ToString();
    }

    private void PrintTable()
    {
        // Absolutely make sure you calibrated the table before printing.

        // Calculate the minimum character amounuts required for columns.
        var dateMinimum = _dateLongest + 3;
        var sizeMinimum = _sizeLongest + 3;

        foreach (var row in _rows)
        {
            Console.Write(row.ShortDateTime);

            // If not enough characters to align the table, fill with whitespaces.
            if (row.ShortDateTime.Length < dateMinimum)
            {
                for (int i = row.ShortDateTime.Length; i < dateMinimum; i++)
                {
                    Console.Write(' ');
                }
            }

            // Do the same to DateTime string.
            Console.Write(row.Size);

            if (row.Size.Length < sizeMinimum)
            {
                for (int i = row.Size.Length; i < sizeMinimum; i++)
                {
                    Console.Write(' ');
                }
            }

            // File name is the last column, do not need to be aligned
            Console.WriteLine(row.FileName);
        }
    }

    // This method performs calibration by iterating through rows and compare them
    // until longest size and file name can be figured out.
    private void Calibrate()
    {
        foreach (var row in _rows)
        {
            if (_sizeLongest < row.Size.Length)
            {
                _sizeLongest = row.Size.Length;
            }

            if (_fileNameLongest < row.FileName.Length)
            {
                _fileNameLongest = row.FileName.Length;
            }
        }
    }

    private void Execute(bool revealHidden)
    {
        // Print DOS-like table title, sans the volume information (too complex).
        Console.WriteLine(Languages.Get("dir_directory_of"), PathSearcher.SystemToShell(_dir));
        Console.WriteLine();

        var windows = OperatingSystem.IsWindows();

        // Iterate through directories.
        foreach (var folder in Directory.GetDirectories(_dir))
        {
            // Acquire info and last changed datetime for the entry.
            var info = new DirectoryInfo(folder);
            var modified = Directory.GetLastWriteTime(folder);

            // Generate text for date time.
            var tableDate = GetTableDateTime(modified);

            // We calibrate date first to save time later.
            if (_dateLongest < tableDate.Length)
            {
                _dateLongest = tableDate.Length;
            }

            // Add to row.
            _rows.Add(new(tableDate, "<DIR>", info.Name));

            // +1 to directory entry count.
            _dirCount++;
        }

        // Do the same for files.
        foreach (var file in Directory.GetFiles(_dir))
        {
            var info = new FileInfo(file);
            var modified = File.GetLastWriteTime(file);

            var tableDate = GetTableDateTime(modified);

            if (_dateLongest < tableDate.Length)
            {
                _dateLongest = tableDate.Length;
            }

            if (!revealHidden)
            {
                if (windows && (info.Attributes.HasFlag(FileAttributes.Hidden)
                    || info.Attributes.HasFlag(FileAttributes.System)))
                {
                    continue;
                }

                if (!windows && info.Name.StartsWith('.'))
                {
                    continue;
                }
            }

            _rows.Add(new(tableDate, info.Length.ToString(), info.Name));
            _fileCount++;
        }

        Calibrate();
        PrintTable();

        // Print out the last line.
        // We do it manually here.
        for (int i = 0; i < _dateLongest; i++)
        {
            Console.Write(' ');
        }

        Console.WriteLine("{0} files, {1} directories", _fileCount, _dirCount);
    }

    [Value("directory", required: false, position: 0)]
    public string? TargetDir { get; set; }

    [Option('d', true)]
    public string? DateFormat { get; set; }

    [Option('t', true)]
    public string? TimeFormat { get; set; }

    [Option('h', false)]
    public bool RevealHidden { get; set; }

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
    {
        // All chunked out ones need fix

        if (!string.IsNullOrWhiteSpace(TargetDir) && Directory.Exists(TargetDir))
        {
            _dir = TargetDir;
        }

        if (!string.IsNullOrWhiteSpace(DateFormat))
        {
            _dateFormat = DateFormat;
        }

        if (!string.IsNullOrWhiteSpace(TimeFormat))
        {
            _timeFormat = TimeFormat;
        }

        Execute(RevealHidden);

        return 0;
    }
}
