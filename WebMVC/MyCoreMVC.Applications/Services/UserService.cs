using Microsoft.EntityFrameworkCore;
using MyCoreMvc.Common;
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
    public class UserService : IUserService
    {
        public readonly IRepository<User, long> _userRepository;
        public UserService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

       
        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<UserDto> GetAll(UserInputDto dto)
        {
            var query = _userRepository.GetAll();
            if (string.IsNullOrEmpty(dto?.SearchInputStr))
            {
                query = query.Where(u => u.Email == dto.SearchInputStr || u.PhoneNum == dto.SearchInputStr || u.UserName == dto.SearchInputStr);
            }
            return AutoMapperExtension.MapTo<User, UserDto>(query).AsQueryable();
        }

        public IQueryable<UserDto> GetAll()
        {
            var query = _userRepository.GetAll();
            return AutoMapperExtension.MapTo<User, UserDto>(query).AsQueryable();
        }

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<UserDto> GetPageList(UserInputDto dto)
        {
            var query = _userRepository.GetAll();
            int count = query.Count();
            if (string.IsNullOrEmpty(dto.SearchInputStr))
            {
                query = query.Where(u => u.Email == dto.SearchInputStr || u.PhoneNum == dto.SearchInputStr || u.UserName == dto.SearchInputStr);
            }
           query = query.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize);
            return AutoMapperExtension.MapTo<User, UserDto>(query).AsQueryable();
        }

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDto Get(long id)
        {
            var user = _userRepository.Get(id);
            if (user != null)
            {
                return AutoMapperExtension.MapTo<User, UserDto>(user);
            }
            return null;
        }

        public UserDto Add(UserDto dto)
        {
           var query= _userRepository.GetAll().Where(r => r.UserName == dto.UserName || r.Email == dto.Email || r.PhoneNum == dto.PhoneNum).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("添加失败，用户名或邮箱或电话已存在！");
            }
            var user= AutoMapperExtension.MapTo<UserDto, User>(dto);
            var result= _userRepository.Insert(user);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<User, UserDto>(result);
            }
            return null;
        }
        /// <summary>
        /// 根据id删除用户
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            _userRepository.Delete(id);
        }
        

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserDto Update(UserDto dto)
        {
            var query = _userRepository.GetAll().Where(r => r.UserName == dto.UserName || r.Email == dto.Email || r.PhoneNum == dto.PhoneNum).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("修改失败，用户名或邮箱或电话已存在！");
            }
            var user = AutoMapperExtension.MapTo<UserDto, User>(dto);
            var result = _userRepository.Update(user);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<User, UserDto>(result);
            }
            return null;
        }










        public bool CheckLogin(string loginName, string password)
        {
            var model = _userRepository.GetAll()?.SingleOrDefault(r => r.UserName == loginName && r.Password == password);
            return model == null ? false : true;
        }

        //public void IncrLoginError(long id)
        //{
        //    User user = Get(id);
        //    user.LastLoginErrorDateTime = DateTime.Now;
        //    user.LoginErrorTimes++;
        //     Update(user);
        //}

        //public void ResetLoginError(long id)
        //{
        //    User user = Get(id);
        //    user.LastLoginErrorDateTime = null;
        //    user.LoginErrorTimes = 0;
        //    Update(user);
        //}

        //public bool IsLocked(long id)
        //{
        //    User user = Get(id);
        //    //错误登录次数>=5，最后一次登录错误时间在30分钟之内
        //    return user.LoginErrorTimes >= 5 && user.LastLoginErrorDateTime > DateTime.Now.AddMinutes(-30);
        //}
    }
}
