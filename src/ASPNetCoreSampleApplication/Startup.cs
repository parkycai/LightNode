using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using LightNode2;
using System.IO;
using System.Reflection;
using System.Net;

namespace ASPNetCoreSampleApplication
{
    /// <summary>
    /// ASP.NET Core Entry Point
    /// </summary>
    /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware?tabs=aspnetcore2x"/>
    public class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </remarks>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Both is same handling. /health/* will be response.
            //app.Map("/health", HandleHealth);
            app.Map("/health", builder =>
            {
                builder.Run(async context => await context.Response.WriteAsync("health"));
            });

            // api response
            app.Map("/api", builder =>
            {
                builder.UseLightNode(typeof(Startup));
            });

            // swagger response
            app.Map("/swagger", builder =>
            {
                var xml = "ASPNetCoreSampleApplication.xml";
                var xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), xml);

                builder.UseLightNodeSwagger(new LightNode2.Swagger.SwaggerOptions("ASPNetCoreSampleApplication", "/api")
                {
                    XmlDocumentPath = xmlPath,
                    IsEmitEnumAsString = true,
                });
            });

            // End of Pipeline
            // 404 : Page not found.
            app.Run(async (context) =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync("Page not found");
            });
        }

        private static void HandleHealth(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("health");
            });
        }
    }
}
