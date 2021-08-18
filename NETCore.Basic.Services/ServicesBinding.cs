using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Repository.Repositories;
using NETCore.Basic.Services.DataServices;
using NETCore.Basic.Services.Mapping;
using NETCore.Basic.Services.Pagination;
using NETCore.Basic.Util.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services
{
    public sealed class ServicesBinding
    {
        public void BindServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, UsersRepository>();

            services.AddSingleton<IHashing, Hashing>();
            services.AddSingleton<IEncryption, Encryption>();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IMapping, UserMapping>();

        }
    }
}
