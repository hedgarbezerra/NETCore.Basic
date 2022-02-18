using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Repository.DataContext;

namespace NETCore.Basic.Repository.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(NetDbContext context)
            : base(context)
        {
        }

    }
}
