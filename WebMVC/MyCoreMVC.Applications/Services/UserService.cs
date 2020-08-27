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

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IQueryable<User> GerAll()
        {
            return _userRepository.GetAll();
        }

        public User Get(int id)
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
    }
}
