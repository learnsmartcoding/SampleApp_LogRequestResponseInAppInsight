using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SampleApp_LogRequestResponseInAppInsight.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp_LogRequestResponseInAppInsight
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
            services.AddControllers();

            services.AddTransient<RequestBodyLoggingMiddleware>();
            services.AddTransient<ResponseBodyLoggingMiddleware>();
            services.AddSwaggerGen(
              options =>
              {
                  options.SwaggerDoc("v1", new OpenApiInfo()
                  {
                      Title = "LearnSmartCoding AppInsight Demo",
                      Version = "V1",
                      Description = "",
                      TermsOfService = new System.Uri("https://karthiktechblog.com/copyright"),
                      Contact = new OpenApiContact()
                      {
                          Name = "Karthik Kannan",
                          Email = "learnsmartcoding@gmail.com",
                          Url = new System.Uri("http://www.karthiktechblog.com")
                      },
                      License = new OpenApiLicense
                      {
                          Name = "Use under LICX",
                          Url = new System.Uri("https://karthiktechblog.com/copyright"),
                      }
                  });
              }
          );
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //use custom middleware for logging request and response
            app.UseRequestBodyLogging();
            app.UseResponseBodyLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "LearnSmartCoding AppInsight DemoV1");
            });
        }
    }
}
