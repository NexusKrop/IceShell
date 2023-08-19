namespace IceShell.Batching;

using IceShell.Core;
using IceShell.Core.CLI.Languages;
using IceShell.Core.Commands;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BatchFile : ICommandExecutor
{
    public BatchFile(IList<string> lines, IDictionary<string, int> labels)
    {
        _lines = lines;
        _labels = labels;
    }

    private readonly IList<string> _lines;
    private readonly IDictionary<string, int> _labels;

    public bool SupportsJump => true;

    public int CurrentLine { get; private set; }

    public static BatchFile Parse(IEnumerable<string> lines)
    {
        var retVal = new List<string>();
        var labels = new Dictionary<string, int>();
        var lineNum = 0;

        foreach (var line in lines)
        {
            lineNum++;

            // Double comments
            if (line.StartsWith("::"))
            {
                retVal.Add(string.Empty);
                continue;
            }

            // Label
            if (line.StartsWith(':'))
            {
                labels.Add(line[1..], lineNum);
                retVal.Add(string.Empty);
                continue;
            }

            retVal.Add(line);
        }

        return new(retVal, labels);
    }

    public void RunBatch(IShell shell)
    {
        while (CurrentLine < _lines.Count)
        {
            var line = _lines[CurrentLine];
            CurrentLine++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (shell.Execute(line, this) != 0)
            {
                throw new CommandFormatException(string.Format(Languages.Get("batch_not_successful"), CurrentLine + 1));
            }
        }
    }

    public void Jump(string label)
    {
        if (!_labels.TryGetValue(label, out var labelLine))
        {
            throw new CommandFormatException(string.Format(Languages.Get("batch_goto_no_such_label"), label));
        }

        if (labelLine > _lines.Count)
        {
            throw new InvalidOperationException("Label is not correctly parsed? Over line amount!");
        }

        CurrentLine = labelLine - 1;
    }
}