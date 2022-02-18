using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Repository.DataContext;

namespace NETCore.Basic.Repository.Repositories
{
    public class LogRepository : BaseRepository<EventLog>, IRepository<EventLog>
    {
        public LogRepository(NetDbContext context)
            : base(context)
        {

        }
    }
}
