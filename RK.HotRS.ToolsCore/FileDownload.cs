using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace RK.HotRS.ToolsCore
{
    [ExcludeFromCodeCoverage]
    public sealed class FileDownload :IDisposable
    {
        public FileDownload() { }

        public MemoryStream MemoryStream { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FullFileName { get; set; }

        public void Dispose() { }
    }
}
