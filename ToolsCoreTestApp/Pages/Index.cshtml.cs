using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RK.HotRS.ToolsCore.Extensions;

namespace ToolsCoreTestApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
			throw new NotImplementedException("oh crap!").SetData(
				new SortedDictionary<string, string>()
				{
					{"Property 3", "Not a number" },
					{"Property 2", "Not an HttpStatusCode" },
					{"Property 1", "Cannot be null" },
				});
        }
    }
}
