using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Repository.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Repository.Repositories
{
    public class LogRepository : BaseRepository<EventLog>, IRepository<EventLog>
    {
        public LogRepository(NetDbContext context)
            :base(context)
        {

        }
    }
}
