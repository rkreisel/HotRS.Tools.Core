using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RK.HotRS.ToolsCore.Middleware.GlobalErrorHandler;

namespace ToolsCoreTestApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			//    if (env.IsDevelopment())
			//    {
			//        app.UseBrowserLink();
			//        app.UseDeveloperExceptionPage();
			//    }
			//    else
			//    {
			//        app.UseExceptionHandler("/Error");
			//    }

			app.UseGlobalExceptionMiddleware(options =>
			{
				options.ContentType = "text/json";
				options.FullDetail = env.IsDevelopment();
				options.StatusCode = HttpStatusCode.InternalServerError;
			});
			app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
