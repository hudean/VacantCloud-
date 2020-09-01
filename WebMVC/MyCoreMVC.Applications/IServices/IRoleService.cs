using MyCoreMvc.Entitys;
using MyCoreMVC.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.IServices
{
    /// <summary>
    /// 角色业务
    /// </summary>
    public interface IRoleService : IBaseService
    {
        IQueryable<Role> GerAll();

        Role Get(int roleId);

        Role Add(Role rore);

        Role Update(Role rore);

        void MarkDeleted(long roleId);
        /// <summary>
        /// 给用户adminuserId增加权限roleIds
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="roleIds"></param>
        void AddRoleIds(long adminUserId, long[] roleIds);

        /// <summary>
        /// 更新权限，先删再加
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="roleIds"></param>
        void UpdateRoleIds(long adminUserId, long[] roleIds);

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <returns></returns>
        RoleDTO[] GetByAdminUserId(long adminUserId);
    }
}
