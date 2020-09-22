using VaCant.Entitys;
using VaCant.Repositorys;
using VaCant.Applications.Dtos;
using VaCant.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace VaCant.Applications.Services
{
    public class UserLogService: IUserLogService
    {
        private readonly IRepository<UserLog> _userLogRepository;
        public UserLogService(IRepository<UserLog> userLogRepository)
        {
            _userLogRepository = userLogRepository;
        }

        public IQueryable<UserLog> GetAll()
        {
            return _userLogRepository.GetAll();
        }

        public IQueryable<UserLog> GetPageList(UserLogInputDto input)
        {
            return _userLogRepository.GetAll().Skip((input.PageIndex-1)*input.PageSize).Take(input.PageSize);
        }
    }
}
