using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreMvc.Repositorys
{
   public interface IUnitOfWork
    {
        DbContext GetDbContext();

        Task<int> SaveChangesAsync();
    }
}
