using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using RK.HotRS.ToolsCore;
using RK.HotRS.ToolsCore.Extensions;
using SIO = System.IO;

namespace RK.HotRS.Tools.Core.IntegrationTests.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ExcelFunctionsTestsController : Controller
    {
        // GET api/values
        [HttpPost(Name = "AddNamedRange")]
        public IActionResult AddNamedRange(IFormFile file, string sheetName, string rangeName, string rangeValue)
        {
            var fn = $"d:\\temp\\{file.FileName}";
            if (!SIO.File.Exists(fn))
            {
                return BadRequest($"Could not find {file.FileName}");
            }
            var fi = new FileInfo(fn);
            using var package = new ExcelPackage(fi);
            var sheet = package.Workbook.Worksheets[sheetName];
            var targetRange = sheet.Cells[5, 2];
            package.Workbook.Names.Add(rangeName, targetRange);
            package.Workbook.SetNamedRangeValue(rangeName, rangeValue, true);
            var ext = ".xlsx";
            var newFn = $"{file.FileName}-updated";
            var fd = new FileDownload
            {
                FileExtension = ext,
                FileName = $"{newFn}",
                FullFileName = $"{newFn}.xlsm",
                MimeType = "application/vnd.ms-excel.sheet.macroEnabled.12"
            };
            fd.MemoryStream = new MemoryStream(package.GetValidByteArray());
            return base.File(fd.MemoryStream, fd.MimeType, fd.FullFileName);
        }
    }
}
