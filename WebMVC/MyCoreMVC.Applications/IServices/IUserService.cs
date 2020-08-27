using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.IServices
{
    public interface IUserService : IBaseService
    {
        IQueryable<User> GerAll();

        User Get(int id);

        User Add(User user);

        User Update(User user);

        void Delete(int id);

        //检查用户名密码是否正确
        bool CheckLogin(string loginName, string password);
    }
}
