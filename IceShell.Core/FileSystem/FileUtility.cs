// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Reflection.PortableExecutable;
using System.Runtime.Versioning;

/// <summary>
/// Provides utilities for file formats and types.
/// </summary>
public static class FileUtility
{
    /// <summary>
    /// Determines whether the specified file is an executable.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns><see langword="true" /> if the specified file is an executable; otherwise, <see langword="false"/>. </returns>
    public static bool IsExecutable(string file)
    {
        if (OperatingSystem.IsWindows())
        {
            try
            {
                using var stream = File.OpenRead(file);
                var reader = new PEReader(stream);

                if (reader.PEHeaders == null)
                {
                    return false;
                }

                return reader.PEHeaders.IsExe;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            using var reader = new BinaryReader(File.OpenRead(file));

            return reader.ReadByte() == 0x7f && reader.ReadChar() == 'E'
                && reader.ReadChar() == 'L'
                && reader.ReadChar() == 'F';
        }
        else
        {
            return false;
        }
    }
}
