using MyCoreMvc.Entitys;
using MyCoreMvc.Repositorys;
using MyCoreMVC.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.Services
{
    public class UserService : IUserService
    {
        public readonly IRepository<User, long> _userRepository;
        public UserService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Add(User user)
        {
            return _userRepository.Insert(user);
        }

        public void Delete(long id)
        {
            _userRepository.Delete(id);
        }

        public IQueryable<User> GerAll()
        {
            return _userRepository.GetAll();
        }

        public User Get(long id)
        {
            return _userRepository.Get(id);
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }


        public bool CheckLogin(string loginName, string password)
        {
            var model = _userRepository.GetAll()?.SingleOrDefault(r => r.Name == loginName && r.Password == password);
            return model == null ? false : true;
        }

        public void IncrLoginError(long id)
        {
            User user = Get(id);
            user.LastLoginErrorDateTime = DateTime.Now;
            user.LoginErrorTimes++;
             Update(user);
        }

        public void ResetLoginError(long id)
        {
            User user = Get(id);
            user.LastLoginErrorDateTime = null;
            user.LoginErrorTimes = 0;
            Update(user);
        }

        public bool IsLocked(long id)
        {
            User user = Get(id);
            //错误登录次数>=5，最后一次登录错误时间在30分钟之内
            return user.LoginErrorTimes >= 5 && user.LastLoginErrorDateTime > DateTime.Now.AddMinutes(-30);
        }
    }
}
