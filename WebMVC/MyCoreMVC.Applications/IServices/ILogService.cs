using VaCant.Entitys;
using VaCant.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaCant.Applications.IServices
{
    public interface ILogService : IBaseService
    {
        IQueryable<Log> GetAll();
        IQueryable<Log> GetPageList(LogInputDto input);
    }
}
