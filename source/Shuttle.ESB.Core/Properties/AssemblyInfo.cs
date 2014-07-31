using System.Reflection;
using System.Runtime.CompilerServices;

#if NET35FULL
[assembly: AssemblyTitle("Shuttle.ESB.Core for .NET Framework 3.5")]
#endif

#if NET40FULL
[assembly: AssemblyTitle("Shuttle.ESB.Core for .NET Framework 4.0")]
#endif

#if NET45FULL
[assembly: AssemblyTitle("Shuttle.ESB.Core for .NET Framework 4.5")]
#endif

[assembly: AssemblyVersion("3.2.2.0")]
[assembly: InternalsVisibleTo("Shuttle.ESB.Test.Shared")]
[assembly: InternalsVisibleTo("Shuttle.ESB.Test.Integration")]
[assembly: InternalsVisibleTo("Shuttle.ESB.Test.Unit")]
