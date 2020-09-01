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

        User Get(long id);

        User Add(User user);

        User Update(User user);

        void Delete(long id);

        //检查用户名密码是否正确
        bool CheckLogin(string loginName, string password);

        /// <summary>
        /// 记录一次登录失败
        /// </summary>
        /// <param name="id"></param>
        void IncrLoginError(long id);

        /// <summary>
        /// 重置登录错误次数和时间/清零
        /// </summary>
        /// <param name="id"></param>
        void ResetLoginError(long id);

        /// <summary>
        /// 判断用户是否已经被锁定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsLocked(long id);
    }
}
