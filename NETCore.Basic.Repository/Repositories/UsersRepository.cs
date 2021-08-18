using Microsoft.EntityFrameworkCore;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Repository.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Basic.Repository.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(NetDbContext context)
            :base(context)
        {
        }

    }
}
