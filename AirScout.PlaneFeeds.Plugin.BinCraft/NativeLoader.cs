using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ZstdNet;

public static class NativeLoader
{
    private static bool loaded = false;

    public static void LoadLibzstd()
    {
        if (loaded) return;

        var assembly = Assembly.GetExecutingAssembly();
        string arch = Environment.Is64BitProcess ? "x64" : "x86";
        string resourceName = $"AirScout.PlaneFeeds.Plugin.BinCraft.libzstd.{arch}.dll"; // adjust namespace!

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new Exception($"Embedded libzstd DLL for {arch} not found: {resourceName}");

            string outputPath = Path.Combine(
                Path.GetDirectoryName(assembly.Location),
                "libzstd.dll"
            );

            using (var outFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(outFile);
            }

            LoadLibrary(outputPath);
        }

        loaded = true;
    }

    [DllImport("kernel32", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);
}
