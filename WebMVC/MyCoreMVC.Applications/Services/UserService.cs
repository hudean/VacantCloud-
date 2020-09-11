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
using System.Threading.Tasks;

namespace MyCoreMVC.Applications.Services
{
    public class UserService : IUserService
    {
        public readonly IRepository<User> _userRepository;
        public readonly IRepository<UserRole> _userRoleRepository;
        public readonly IRepository<Role> _roleRepository;
        public UserService(IRepository<User> userRepository, IRepository<UserRole> userRoleRepository, IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
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
            query = query.Include(r => r.UserRoles);
            var model = query.Select(r =>
                new UserDto
                {
                    Address = r.Address,
                    CreationTime = r.CreationTime,
                    CreatorUserId = r.CreatorUserId,
                    Email = r.Email,
                    Id = r.Id,
                    IsDelete = r.IsDelete,
                    LastLoginErrorDateTime = r.LastLoginErrorDateTime,
                    LoginErrorTimes = r.LoginErrorTimes,
                    Password = r.Password,
                    PasswordSalt = r.PasswordSalt,
                    PhoneNum = r.PhoneNum,
                    UserName = r.UserName,
                    RoleIds = r.UserRoles.Select(t => t.RoleId).ToList(),
                    RoleNames = r.UserRoles.Select(t => t.Role.RoleName).ToList()
                });
            return model;//AutoMapperExtension.MapTo<User, UserDto>(query).AsQueryable();
        }

        public IQueryable<User> GetAll()
        {
            var query = _userRepository.GetAll();
            return query;
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
        public async Task<UserDto> GetAsync(long id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user != null)
            {
                UserDto userDto = AutoMapperExtension.MapTo<User, UserDto>(user);
                await _userRoleRepository.GetAll().Where(r => r.UserId == id).Include(r => r.UserId == id).ForEachAsync(r =>
                {
                    userDto.RoleIds.Add(r.RoleId);
                    userDto.RoleNames.Add(r.Role.RoleName);
                });
                return userDto;
            }
            return null;
        }

        public async Task<UserDto> AddAsync(UserDto inputDto)
        {
            var query = _userRepository.GetAll().Where(r => r.UserName == inputDto.UserName || r.Email == inputDto.Email || r.PhoneNum == inputDto.PhoneNum).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("添加失败，用户名或邮箱或电话已存在！");
            }
            var user = AutoMapperExtension.MapTo<UserDto, User>(inputDto);
            var roIds = inputDto.RoleIds;
            var result= await _userRepository.InsertAsync(user);
           
            if (result != null)
            {
                roIds.ForEach(async r => {
                    await _userRoleRepository.InsertAsync(new UserRole()
                    {
                        RoleId = r,
                        UserId = result.Id
                    });

                });
                return inputDto;
            }
            return null;
        }
        /// <summary>
        /// 根据id删除用户
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(id);
        }


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserDto> UpdateAsync(UserDto inputDto)
        {
            var query = _userRepository.GetAll().Where(r => r.UserName == inputDto.UserName || r.Email == inputDto.Email || r.PhoneNum == inputDto.PhoneNum).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("修改失败，用户名或邮箱或电话已存在！");
            }
            var user = AutoMapperExtension.MapTo<UserDto, User>(inputDto);
            var roIds = inputDto.RoleIds;
            var userRoles = await _userRoleRepository.GetAllListAsync(t => t.UserId == inputDto.Id);
            userRoles.ForEach(async t =>
            {
                if (!inputDto.RoleIds.Contains(t.RoleId))
                {
                    await _userRoleRepository.DeleteAsync(t);
                }
                else
                {
                    roIds.Remove(t.RoleId);
                }


            });
            roIds.ForEach(async r => {
                await _userRoleRepository.InsertAsync(new UserRole()
                {
                    RoleId = r,
                    UserId = inputDto.Id
                });

            });
            var result = await _userRepository.UpdateAsync(user);
            if (result != null)
            {
                return inputDto;
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
