using MyCoreMvc.Entitys;
using MyCoreMVC.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.IServices
{
    public interface ILogService : IBaseService
    {
        IQueryable<Log> GetAll();
        IQueryable<Log> GetPageList(LogInputDto input);
    }
}
