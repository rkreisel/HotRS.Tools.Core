using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using RK.HotRS.ToolsCore.Middleware.GlobalErrorHandler;
using RK.HotRS.ToolsCore.Middleware.SwaggerTools;

namespace RK.HotRS.Tools.Core.IntegrationTests
{
    public class Startup
    {
		const string TITLE = "RK HotRS Tools Integration Test App";
		const string VERSION = "v2";
        public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddSwaggerGen(c=>
				{
					c.SwaggerDoc("v2", new OpenApiInfo { Title = TITLE , Version = VERSION});
                    c.OperationFilter<FormFileParameter>();
                    var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{PlatformServices.Default.Application.ApplicationName}.xml");
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }                    
                });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSingleton(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
			app.UseGlobalExceptionMiddleware(
				options =>
				{
					options.ContentType = this.Configuration.GetValue<string>("GlobalErrorHandlerOptions:ContentType");
					options.FullDetail = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:FullDetail");
					options.IncludeData = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:IncludeData");
					options.IncludeHelpLink = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:IncludeHelpLink");
					options.IncludeHResult = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:IncludeHResult");
					options.IncludeInnerException = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:IncludeInnerException");
					options.IncludeSource = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:IncludeSource");
					options.IncludeStackTrace = this.Configuration.GetValue<bool>("GlobalErrorHandlerOptions:IncludeStackTrace");
				});

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v2/swagger.json", TITLE);
			});
			app.UseMvc();
		}
    }
}
