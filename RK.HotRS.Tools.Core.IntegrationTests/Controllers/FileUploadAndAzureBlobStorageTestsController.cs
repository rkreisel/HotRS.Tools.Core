using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RK.HotRS.ToolsCore.Attributes;
using RK.HotRS.ToolsCore.Helpers.FileUpload;

namespace RK.HotRS.Tools.Core.IntegrationTests.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileUploadAndAzureBlobStorageTestsController : Controller
    {
        private static readonly FormOptions defaultFormOptions = new FormOptions();

        // GET api/values
        [HttpPost(Name = "UploadSmallFile")]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadSmallFile(string targetFolder,  IFormFile uploadedFile)
        {
            if (string.IsNullOrWhiteSpace(targetFolder))
            {
                return BadRequest($"Target folder parameter was null or empty.");
            }
            if (!Directory.Exists(targetFolder))
            {
                return NotFound($"Target folder ({targetFolder}) not found.");
            }
            if (uploadedFile.Length > 4000000)
            {
                return BadRequest($"{uploadedFile.FileName} is too large. Max size is 4mb.");
            }
            if (!MultipartRequestHelper.IsMultipartContentType(this.Request.ContentType))
            {
                return BadRequest($"Expected Multipart request, but received {this.Request.ContentType}.");
            }
            var ifh = new FileUploadHelper();
            if(await ifh.UploadSmallFileAsync(uploadedFile, targetFolder))
            {
                return Ok($"{Path.Combine(targetFolder, uploadedFile.FileName)}");
            }

            return BadRequest("Unexpected error occured.");
        }

        /// <summary>
        /// Upload a file of any size. Note that the file MUST be provided in the HTTPContext so it cannot be 
        /// specified in the parameters. Try testing via Postman.
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        [HttpPost(Name = "UploadAnySizeFile")]
        [DisableFormValueModelBinding]
        [GenerateAntiforgeryTokenCookieForAjax]
        public async Task<IActionResult> UploadAnySizeFile(string targetFolder)
        {
            if (string.IsNullOrWhiteSpace(targetFolder))
            {
                return BadRequest($"Target folder parameter was null or empty.");
            }
            if (!Directory.Exists(targetFolder))
            {
                return NotFound($"Target folder ({targetFolder}) not found.");
            }
            if (!MultipartRequestHelper.IsMultipartContentType(this.Request.ContentType))
            {
                return BadRequest($"Expected multi-part content. Received {this.Request.ContentType}");
            }
            var ifh = new FileUploadHelper();
            var result = await ifh.UploadAsync(this.HttpContext, defaultFormOptions, targetFolder);
            return Ok(result);
        }
    }
}
