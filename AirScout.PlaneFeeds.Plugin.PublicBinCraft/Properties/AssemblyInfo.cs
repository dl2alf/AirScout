using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("AirScout.PlaneFeeds.Plugin.PublicBinCraft")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AirScout.PlaneFeeds.Plugin.PublicBinCraft")]
[assembly: AssemblyCopyright("Copyright ©  2026")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("a00d22fa-663d-48c3-9a38-8b19c9fc49c6")]

// AirScout.exe's plugin loader (MapDlg.LoadPlugins) only keeps plugins whose
// Version starts with the running app's major.minor - keep this in step with
// AirScout/Properties/AssemblyInfo.cs's AssemblyVersion, or the plugin will
// compose successfully via MEF but silently never appear in the feed dropdown.
[assembly: AssemblyVersion("1.4.5.0")]
[assembly: AssemblyFileVersion("1.4.5.0")]
