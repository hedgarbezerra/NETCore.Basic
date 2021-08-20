using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.Basic.Repository.DataContext;
using NETCore.Basic.Services;
using NETCore.Basic.Services.Mapping;
using NETCore.Basic.Util.Configuration;
using NETCore.Basic.Util.Crypto;
using NETCore.Basic.Util.Helper;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NETCore.Basic.API
{
    public class Startup
    {
        private readonly APIConfigurations _apiConfigurations;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _apiConfigurations = new APIConfigurations(configuration);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers()
                .AddJsonOptions(ops =>
                {
                    ops.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                    ops.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    ops.JsonSerializerOptions.IgnoreNullValues = false;
                    ops.JsonSerializerOptions.WriteIndented = true;
                    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    ops.JsonSerializerOptions.MaxDepth = 64; //max
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDirectoryBrowser();

            ServicesBinding binding = new ServicesBinding(_apiConfigurations);
            binding.BindServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Setting up exception handling

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(c => c.Run(async context =>
                {
                    var exception = context.Features
                        .Get<IExceptionHandlerPathFeature>()
                        .Error;
                    var result = JsonConvert.SerializeObject(new { message = exception.Message, stacktrace = exception.StackTrace });
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(result);
                }));
            }
            #endregion


            #region setting up logging and log browser visualization

            Log.Logger = new LoggerConfiguration()
                       .Enrich.FromLogContext()
                       .WriteTo.MSSqlServer(_apiConfigurations.ConnectionString,
                       autoCreateSqlTable: true,
                       restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                       appConfiguration: Configuration,
                       tableName: _apiConfigurations.LoggingTable)
                       .CreateLogger();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "logs")),
                RequestPath = "/Log"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "logs")),
                RequestPath = "/Log"
            });
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Comments and examples for logging
            //Adding a customColumn to Logger
            //app.Use(async (httpContext, next) =>
            //{
            //    var userName = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : string.Empty;
            //    LogContext.PushProperty("Username", userName);
            //    await next.Invoke();
            //});

            //Logging to file
            // Example .WriteTo.File(env.WebRootPath + "\\logs\\log_.txt", rollingInterval: RollingInterval.Minute)
            //  .WriteTo.File(new RenderedCompactJsonFormatter(), env.WebRootPath+ "\\logs\\log.json")

            #endregion

        }
    }
}
