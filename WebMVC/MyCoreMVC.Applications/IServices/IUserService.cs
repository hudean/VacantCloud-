using MyCoreMvc.Entitys;
using MyCoreMVC.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreMVC.Applications.IServices
{
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<UserDto> GetAll(UserInputDto dto);

        public IQueryable<User> GetAll();
        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<UserDto> GetPageList(UserInputDto dto);

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserDto> GetAsync(long id);

        public Task<UserDto> AddAsync(UserDto dto);
        /// <summary>
        /// 根据id删除用户
        /// </summary>
        /// <param name="id"></param>
        public Task DeleteAsync(long id);


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<UserDto> UpdateAsync(UserDto dto);

        ////检查用户名密码是否正确
        //bool CheckLogin(string loginName, string password);

        ///// <summary>
        ///// 记录一次登录失败
        ///// </summary>
        ///// <param name="id"></param>
        //void IncrLoginError(long id);

        ///// <summary>
        ///// 重置登录错误次数和时间/清零
        ///// </summary>
        ///// <param name="id"></param>
        //void ResetLoginError(long id);

        ///// <summary>
        ///// 判断用户是否已经被锁定
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //bool IsLocked(long id);
    }
}
