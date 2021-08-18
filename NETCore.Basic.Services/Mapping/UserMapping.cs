using AutoMapper;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services.Mapping
{
    public class UserMapping : IMapping
    {
        public void Map(IMapperConfigurationExpression config)
        {
            config.CreateMap<User, InputUser>();
            config.CreateMap<OutputUser, User>();
            config.CreateMap<User, OutputUser>();
        }
    }
}
