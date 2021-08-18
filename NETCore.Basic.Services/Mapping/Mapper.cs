using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services.Mapping
{
    public interface IClassMapper
    {
        void Map();
    }
    public sealed class Mapper : IClassMapper
    {
        public IServiceCollection _services { get; }
        public IEnumerable<IMapping> _maps { get; }
        public Mapper(IServiceCollection services, IEnumerable<IMapping> maps)
        {
            _services = services;
            _maps = maps;
        }

        public void Map()
        {
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var map in _maps)
                {
                    map.Map(cfg);
                }
            });

            IMapper mapper = config.CreateMapper();

            _services.AddSingleton(mapper);
        }
    }
}
