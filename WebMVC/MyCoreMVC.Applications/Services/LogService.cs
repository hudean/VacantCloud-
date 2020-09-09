using MyCoreMvc.Entitys;
using MyCoreMvc.Repositorys;
using MyCoreMVC.Applications.Dtos;
using MyCoreMVC.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.Services
{
    public class LogService : ILogService
    {
        private readonly IRepository<Log> _logRepository;
        public LogService(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        public IQueryable<Log> GetAll()
        {
           return _logRepository.GetAll();
        }

        public IQueryable<Log> GetPageList(LogInputDto input)
        {
            return _logRepository.GetAll().Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize);
        }
    }
}
