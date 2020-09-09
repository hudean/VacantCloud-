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
    /// <summary>
    /// 角色业务
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole,int> _userRoleRepository;
        public RoleService(IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<RoleDto> GetAll(RoleInputDto dto)
        {
            var query = _roleRepository.GetAll(); 
            if (string.IsNullOrEmpty(dto?.SearchRoleName))
            {
                query = query.Where(u => u.RoleName.Contains(dto.SearchRoleName));
            }
            return AutoMapperExtension.MapTo<Role, RoleDto>(query).AsQueryable();
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public IQueryable<RoleDto> GetAll()
        {
            var query = _roleRepository.GetAll();
            return AutoMapperExtension.MapTo<Role, RoleDto>(query).AsQueryable();
        }

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<RoleDto> GetPageList(RoleInputDto dto)
        {
            var query = _roleRepository.GetAll();
            int count = query.Count();
            if (string.IsNullOrEmpty(dto?.SearchRoleName))
            {
                query = query.Where(u => u.RoleName.Contains(dto.SearchRoleName));
            }
            query = query.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize);
            return AutoMapperExtension.MapTo<Role, RoleDto>(query).AsQueryable();
        }

        /// <summary>
        /// 根据id获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleDto Get(long id)
        {
            var role = _roleRepository.Get(id);
            if (role != null)
            {
                return AutoMapperExtension.MapTo<Role, RoleDto>(role);
            }
            return null;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public RoleDto Add(RoleDto dto)
        {
            var query = _roleRepository.GetAll().Where(r => r.RoleName.Contains(dto.RoleName)).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("添加失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<RoleDto, Role>(dto);
            var result = _roleRepository.Insert(role);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<Role, RoleDto>(result);
            }
            return null;
        }
        /// <summary>
        /// 根据id删除角色
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            _roleRepository.Delete(id);
        }


        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public RoleDto Update(RoleDto dto)
        {
            var query = _roleRepository.GetAll().Where(r => r.RoleName.Contains(dto.RoleName)&&r.Id!=dto.Id).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("修改失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<RoleDto, Role>(dto);
            var result = _roleRepository.Update(role);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<Role, RoleDto>(result);
            }
            return null;
        }

    }
}
