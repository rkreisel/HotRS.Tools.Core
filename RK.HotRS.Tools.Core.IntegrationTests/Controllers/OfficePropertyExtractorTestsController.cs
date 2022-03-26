using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RK.HotRS.ToolsCore.Exceptions;
using RK.HotRS.ToolsCore.Extensions;
using RK.HotRS.ToolsCore.Helpers.Office;

namespace RK.HotRS.Tools.Core.IntegrationTests.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OfficePropertyExtractorTestsController : Controller
    {
        // GET api/values
        [HttpGet(Name = "GetPropertiesFrom")]
        public IActionResult GetPropertiesFrom([FromForm] IFormFile file)
        {
			var props = new PropertyHelper().GetProperties(file.Name);
			return Ok(props);
		}       
    }
}
