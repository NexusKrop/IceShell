namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

public record struct ComplexValueDefinition(string Name, bool Required = false);