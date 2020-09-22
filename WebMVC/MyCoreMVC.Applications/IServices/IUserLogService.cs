using VaCant.Entitys;
using VaCant.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaCant.Applications.IServices
{
    public interface IUserLogService : IBaseService
    {
        IQueryable<UserLog> GetAll();
        IQueryable<UserLog> GetPageList(UserLogInputDto input);
    }
}
