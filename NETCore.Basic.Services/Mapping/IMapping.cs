using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services.Mapping
{
    public interface IMapping
    {
        void Map(IMapperConfigurationExpression config);
    }
}
