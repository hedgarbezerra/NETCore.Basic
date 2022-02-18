using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace NETCore.Basic.Services.Mapping
{
    public interface IClassMapper
    {
        void Map(IServiceCollection services);
    }
    public sealed class Mapper : IClassMapper
    {
        public IEnumerable<IMapping> _maps { get; }
        public Mapper(IEnumerable<IMapping> maps)
        {
            _maps = maps;
        }

        public void Map(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var map in _maps)
                {
                    map.Map(cfg);
                }
            });

            IMapper mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
