using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RK.HotRS.ToolsCore.Exceptions;
using RK.HotRS.ToolsCore.Extensions;

namespace RK.HotRS.Tools.Core.IntegrationTests.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GlobalErrorHandlerTestsController : Controller
    {
        // GET api/values
        [HttpGet(Name = "FormatUncaughtException")]
        public Task<IActionResult> FormatUncaughtException()
        {
			throw new Exception("Error Occured", new Exception("InnerException", new Exception("really inner exception").SetHelpLink("https://anotherhelplink.com")))
				.SetData(new Dictionary<string, string> { { "Error Detail 1", "Detail 1" }, { "Error Detail 2", "Detail 2" } })
				.SetHelpLink("https://trends.google.com/trends/hottrends");

		}       
    }
}
