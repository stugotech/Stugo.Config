using System.Reflection;

[assembly: AssemblyDescription("Utility classes for config")]
[assembly: AssemblyCompany("Stugo Ltd")]
[assembly: AssemblyProduct("Stugo.Config")]
[assembly: AssemblyCopyright("Copyright © Stugo Ltd 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
