using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.Basic.Repository.DataContext;
using NETCore.Basic.Services;
using NETCore.Basic.Services.Mapping;
using NETCore.Basic.Util.Crypto;
using NETCore.Basic.Util.Helper;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Data;
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
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            ServicesBinding binding = new ServicesBinding(_apiConfigurations);
            binding.BindServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
#pragma warning disable CS0618
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.MSSqlServer(_apiConfigurations.ConnectionString,
                        autoCreateSqlTable: true,
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                        appConfiguration: Configuration,
                        tableName: _apiConfigurations.LoggingTable)
                        // Example .WriteTo.File("logs\\meuLogapp.txt", rollingInterval: RollingInterval.Minute)
                        .CreateLogger();
#pragma warning restore CS0618 

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //Adding a customColumn to Logger
            //app.Use(async (httpContext, next) =>
            //{
            //    var userName = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : string.Empty;
            //    LogContext.PushProperty("Username", userName);
            //    await next.Invoke();
            //});
        }
    }
}
