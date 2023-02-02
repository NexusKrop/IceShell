namespace NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

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
