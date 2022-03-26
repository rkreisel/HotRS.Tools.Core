using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;

namespace RK.HotRS.ToolsCore.Helpers.FileUpload
{
	/// <summary>
	/// Interface for FileUploadHelper
	/// </summary>
	public interface IFileUploadHelper
	{
		/// <summary>
		/// Determines the encoding type of the MultipartSection
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		Encoding GetEncoding(MultipartSection section);
		
		/// <summary>
		/// Uploads a file contained in the HTTP message
		/// </summary>
		/// <param name="context">The HTTP Context which includes the file</param>
		/// <param name="defaultFormOptions">A formoptions object. Usually a simple new FormOptions() will suffice</param>
		/// <param name="fileStorePath">The pyhsical path were the file shoudl be written. Note that you may want to 
		/// implement some sort of cleanup method to keep this folder from becomming too large.</param>
		/// <returns>A string with the location of the uploaded file.</returns>
		Task<string> UploadAsync(HttpContext context, FormOptions defaultFormOptions, string fileStorePath);

        Task<bool> UploadSmallFileAsync(IFormFile file, string landingPath);
    }
}