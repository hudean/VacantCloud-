using MyCoreMvc.Entitys;
using MyCoreMVC.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.IServices
{
    public interface IUserLogService : IBaseService
    {
        IQueryable<UserLog> GetAll();
        IQueryable<UserLog> GetPageList(UserLogInputDto input);
    }
}
