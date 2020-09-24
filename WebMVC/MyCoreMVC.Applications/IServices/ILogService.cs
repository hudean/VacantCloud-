using System.Linq;
using VaCant.Applications.Dtos;
using VaCant.Entitys;

namespace VaCant.Applications.IServices
{
    public interface ILogService : IBaseService
    {
        IQueryable<Log> GetAll();

        IQueryable<Log> GetPageList(LogInputDto input);
    }
}