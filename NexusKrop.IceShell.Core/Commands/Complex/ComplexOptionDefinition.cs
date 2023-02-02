namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record struct ComplexOptionDefinition(char ShortName, bool HasValue, bool Required = false);
