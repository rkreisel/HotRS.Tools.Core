using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using RK.HotRS.ToolsCore.Exceptions;
using RK.HotRS.ToolsCore.Extensions;
using RK.HotRS.ToolsCore.Properties;

namespace RK.HotRS.ToolsCore.Helpers.FileUpload
{
    /// <summary>
    /// Part of the helper class for the file uploading
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FileUploadHelper : IFileUploadHelper
    {
        /// <summary>
        /// Uploads a file contained in the HTTP message.
        /// This helper was created using input from <para>&#160;</para>
        /// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-2.2#uploading-large-files-with-streaming and
        /// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.1<para />
        /// If you get HTTP 404.13 errors it means the file is too large. The default maximum size is roughly 28.5mb.
        /// It can be changed in the web.config file:<para />
        /// &lt;system.webServer&gt;<para />
        ///     &lt;security&gt;<para />
        ///         &lt;requestFiltering&gt;<para />
        ///             &lt;!-- This will handle requests up to 50MB --&gt;<para />
        ///             &lt;requestLimits maxAllowedContentLength = "52428800" /&gt;<para />
        ///         &lt;/requestFiltering &gt;<para />
        ///     &lt;/security &gt;<para />
        /// &lt;/system.webServer &gt;<para />
        /// </summary>
        /// <param name="context">The HTTP Context which includes the file</param>
        /// <param name="_defaultFormOptions">A formoptions object. Usually a simple new FormOptions() will suffice</param>
        /// <param name="fileStorePath">The physical path were the file should be written. Note that you may want to 
        /// implement some sort of cleanup method to keep this folder from becomming too large.</param>
        /// <returns>A string with the location of the uploaded file.</returns>
        public async Task<string> UploadAsync(HttpContext context, FormOptions _defaultFormOptions, string fileStorePath)
        {
            context.CheckForNull(nameof(context));
            _defaultFormOptions.CheckForNull(nameof(_defaultFormOptions));

            // Used to accumulate all the form url encoded key value pairs in the request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(context.Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, context.Request.Body);

            var section = await reader.ReadNextSectionAsync().ConfigureAwait(true);
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var filename = contentDisposition.FileName.Value;
                        if (string.IsNullOrWhiteSpace(filename))
                        {
                            throw new InvalidDataException(Resources.FILENAMENOTINCONTENTDISPOSITION);
                        }
                        targetFilePath = Path.Combine(fileStorePath, filename);
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream).ConfigureAwait(false);
                            //_logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.ToString(), value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new RKToolsException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                _ = await reader.ReadNextSectionAsync().ConfigureAwait(false);

                return targetFilePath;
            }
            return null;
        }

        /// <summary>
        /// Upload a file (4mb or less) using  FormFile object.
        /// </summary>
        /// <param name="file">IFromFile object</param>
        /// <param name="landingPath">Where to put the file. (must be an existing path to which the caller has permission to write</param>
        /// <returns></returns>
        public async Task<bool> UploadSmallFileAsync(IFormFile file, string landingPath)
        {
            file.CheckForNull(nameof(file));
            if (string.IsNullOrWhiteSpace(landingPath))
            {
                throw new ArgumentNullException(nameof(landingPath));
            }
            if (!Directory.Exists(landingPath))
            {
                throw new ApplicationException($"LandingPath {landingPath} not found.");
            }
            using (var outputFile = File.Create(Path.Combine(landingPath, file.FileName)))
            {
                await file.OpenReadStream().CopyToAsync(outputFile).ConfigureAwait(false);
            }
            return true;
        }

        /// <summary>
        /// Determines the encoding type of the MultipartSection
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Encoding GetEncoding(MultipartSection section)
        {
            section.CheckForNull(nameof(section));
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}
