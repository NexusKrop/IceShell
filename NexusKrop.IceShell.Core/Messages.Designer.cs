﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NexusKrop.IceShell.Core {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NexusKrop.IceShell.Core.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command not found.
        /// </summary>
        internal static string BadCommand {
            get {
                return ResourceManager.GetString("BadCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Executable not found.
        /// </summary>
        internal static string BadFile {
            get {
                return ResourceManager.GetString("BadFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory of {0}.
        /// </summary>
        internal static string DirDirectory {
            get {
                return ResourceManager.GetString("DirDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Empty command.
        /// </summary>
        internal static string EmptyCommand {
            get {
                return ResourceManager.GetString("EmptyCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File not in an executable (runnable) format.
        /// </summary>
        internal static string FileNotExecutable {
            get {
                return ResourceManager.GetString("FileNotExecutable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No argument specified (command requires {0}).
        /// </summary>
        internal static string NoArguments {
            get {
                return ResourceManager.GetString("NoArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No argument specified.
        /// </summary>
        internal static string NoArgumentsSimple {
            get {
                return ResourceManager.GetString("NoArgumentsSimple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WARNING: Environment variable PATH not found..
        /// </summary>
        internal static string NoPathWarning1 {
            get {
                return ResourceManager.GetString("NoPathWarning1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This could be caused by your system, or a bad profile script..
        /// </summary>
        internal static string NoPathWarning2 {
            get {
                return ResourceManager.GetString("NoPathWarning2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Too less arguments (command requires {0}).
        /// </summary>
        internal static string TooLessArguments {
            get {
                return ResourceManager.GetString("TooLessArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Too many arguments (command requires {0}).
        /// </summary>
        internal static string TooManyArguments {
            get {
                return ResourceManager.GetString("TooManyArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexcepted error: {0}.
        /// </summary>
        internal static string UnexceptedError {
            get {
                return ResourceManager.GetString("UnexceptedError", resourceCulture);
            }
        }
    }
}
