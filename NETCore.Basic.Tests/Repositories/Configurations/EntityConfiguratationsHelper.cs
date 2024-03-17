using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Tests.Repositories.Configurations
{
    internal static class EntityConfiguratationsHelper
    {
        public static EntityTypeBuilder<T> GetEntityBuilder<T>() where T : class
        {
            var entityType = new EntityType(typeof(T).Name, typeof(T), new Model(),  ConfigurationSource.Convention);

            var builder = new EntityTypeBuilder<T>(entityType);
            return builder;
        }
    }
}
