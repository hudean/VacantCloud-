using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VaCant.Repositorys
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        DbContext GetDbContext();

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}