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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NETCore.Basic.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NetDbContext>(opt => opt.UseSqlServer(Configuration.GetSection("ConnectionString").Value));
            services.AddControllers()
                .AddJsonOptions(ops =>
                {
                    ops.JsonSerializerOptions.IgnoreNullValues = false;
                    ops.JsonSerializerOptions.WriteIndented = true;
                    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            ServicesBinding binding = new ServicesBinding();
            binding.BindServices(services);
            var maps = new List<IMapping>();

            Mapper autoMapperMaps = new Mapper(services, maps);
            autoMapperMaps.Map();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}