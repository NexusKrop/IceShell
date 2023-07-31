// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Reflection.PortableExecutable;

public static class FileUtil
{
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
