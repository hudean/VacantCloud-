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
        public RoleService(IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public Role Add(Role role)
        {
            return _roleRepository.Insert(role);
        }

        /// <summary>
        /// 为管理用户添加角色
        /// </summary>
        /// <param name="adminUserId">管理用户ID</param>
        /// <param name="roleIds">角色ID集合</param>
        public void AddRoleIds(long adminUserId, long[] roleIds)
        {
            var adminUser = _userRepository.Get(adminUserId);
            if (adminUser == null)
            {
                throw new ArgumentException();
            }
            var roles = _roleRepository.GetAll().AsNoTracking().Where(r => roleIds.Contains(r.Id)).ToList();
            roles.ForEach(r => { adminUser.Roles.Add(r); });

        }

        public void Delete(int id)
        {
            _roleRepository.Delete(id);
        }

        public IQueryable<Role> GerAll()
        {
            return _roleRepository.GetAll();
        }

        public Role Get(int id)
        {
            return _roleRepository.Get(id);
        }

        public RoleDTO[] GetByAdminUserId(long adminUserId)
        {
            var adminUser = _userRepository.Get(adminUserId);
            if (adminUser == null)
            {
                throw new ArgumentException("不存在的管理员" + adminUserId);
            }
           var roles= adminUser.Roles.ToList();
            return AutoMapperExtension.MapTo<Role, RoleDTO>(roles).ToArray();
        }

        public void MarkDeleted(long roleId)
        {
            _roleRepository.Delete(roleId);
        }

        public Role Update(Role user)
        {
           return _roleRepository.Update(user);
        }

        public void UpdateRoleIds(long adminUserId, long[] roleIds)
        {
            throw new NotImplementedException();
        }
    }
}
