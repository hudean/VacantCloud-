using System.Linq;
using VaCant.Applications.Dtos;
using VaCant.Entitys;

namespace VaCant.Applications.IServices
{
    public interface IUserLogService : IBaseService
    {
        IQueryable<UserLog> GetAll();

        IQueryable<UserLog> GetPageList(UserLogInputDto input);
    }
}