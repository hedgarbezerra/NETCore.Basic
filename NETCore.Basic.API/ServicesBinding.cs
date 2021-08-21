using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Repository.DataContext;
using NETCore.Basic.Repository.Repositories;
using NETCore.Basic.Services.Data;
using NETCore.Basic.Services.External;
using NETCore.Basic.Services.Mapping;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Services.Validation;
using NETCore.Basic.Tests.Services.External;
using NETCore.Basic.Util.Configuration;
using NETCore.Basic.Util.Crypto;
using NETCore.Basic.Util.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.API
{
    public sealed class ServicesBinding
    {
        public ServicesBinding(IAPIConfigurations config, IConfiguration configuration)
        {
            _apiConfig = config;
            _config = configuration;
        }
        public IAPIConfigurations _apiConfig { get; }
        public IConfiguration _config { get; }

        public void BindServices(IServiceCollection services)
        {
            services.AddTransient<IAPIConfigurations, APIConfigurations>();

            #region Helpers
            services.AddScoped<IFileHandler<HtmlDocument>, HTMLHandler>();
            services.AddScoped<IFileHandler<Stream>, FileHandler>();
            services.AddScoped<IHTMLHandler, HTMLHandler>();
            services.AddScoped<ILocalFileHandler, FileHandler>();
            services.AddSingleton<IHashing, Hashing>();
            services.AddSingleton<IEncryption, Encryption>();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            #endregion

            #region Mapping
            //Always add all IMapping implementations before
            services.AddTransient<IMapping, UserMapping>();

            var maps = new List<IMapping>()
                {
                    new UserMapping()
                };
            Mapper autoMapper = new Mapper(maps);
            autoMapper.Map(services);

            #endregion

            #region Validators
            services.AddTransient<IValidator<User>, UserValidation>();

            #endregion

            #region Repositories
            services.AddDbContext<NetDbContext>(opt => opt.UseSqlServer(_apiConfig.ConnectionString));
            services.AddScoped<IRepository<User>, UsersRepository>();
            services.AddScoped<IRepository<EventLog>, LogRepository>();

            #endregion
            #region Services
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddTransient<IHttpConsumer, HttpConsumer>();
            services.AddTransient<IAzureStorage, AzureStorage>((sp) => new AzureStorage(_config["AZR_STORAGE_KEY"], _config["AZR_STORAGE_CONNSTR"], sp.GetService<ILocalFileHandler>()));
            services.AddTransient<IMailing, Mailing>();

            #endregion

            var emailConfigSection = _config.GetSection("EmailSettings");

            services.AddScoped(sp =>  emailConfigSection.Get(typeof(EmailSettings), options => options.BindNonPublicProperties = true));



            #region Exemplifing case of multiple interface implementation GOTO UsersController for more.
            //services.AddTransient<Func<EMappingType, IMapping>>(serviceProvider => key =>
            //{
            //    switch (key)
            //    {
            //        case EMappingType.Example:
            //            return serviceProvider.GetService<ExampleMapping>();
            //        case EMappingType.User:
            //            return serviceProvider.GetService<UserMapping>();
            //        default:
            //            return null;
            //    }
            //});
            #endregion
        }
    }
}
