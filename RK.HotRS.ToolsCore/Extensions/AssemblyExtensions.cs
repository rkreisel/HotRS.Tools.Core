using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RK.HotRS.ToolsCore.Extensions
{
    public static class AssemblyExtensions
    {
		/// <summary>
		/// Returns a string with the contents of the embedded file. Call with Assembly.GetExecutingAssembly().GetTextFileFromAssembly(filename)
		/// </summary>
		/// <param name="asm">The executing assembly - must be a concrete instance (i.e. Assembly.GetExecutingAssembly()</param>
		/// <param name="filename">The (case-sensitive) name of the file to locate.</param>
		/// <returns>A string or an exception.</returns>
		public static string GetTextFileFromAssembly(this Assembly asm, string filename)
        {
            asm.CheckForNull(nameof(asm));
            string result;
            if (!asm.GetManifestResourceNames().AsEnumerable().Any(f => f == $"{asm.GetName().Name}.{filename}"))
                throw new FileNotFoundException($"{filename} does not exist in the assembly.");

            using (Stream rsrcStream = asm.GetManifestResourceStream($"{asm.GetName().Name}.{filename}"))
            {
                using (StreamReader sRdr = new StreamReader(rsrcStream))
                {
                    result = sRdr.ReadToEnd();
                }
            }
            return result;
        }
    }
}
