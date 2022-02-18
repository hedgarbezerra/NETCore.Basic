using AutoMapper;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Models.Users;

namespace NETCore.Basic.Services.Mapping
{
    public class UserMapping : IMapping
    {
        public void Map(IMapperConfigurationExpression config)
        {
            config.CreateMap<User, InputUser>();
            config.CreateMap<InputUser, User>();
            config.CreateMap<OutputUser, User>();
            config.CreateMap<User, OutputUser>();
        }
    }
}
