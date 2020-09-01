using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMVC.Applications.IServices
{
    public interface IPermissionService : IBaseService
    {
        public IQueryable<Permission> GetAll();
        public Permission Get(int id);

        public Permission Add(Permission permission);

        public Permission Edit(Permission permission);

        public int Delete(int id);
        public int Delete(Permission permission);

        //获取角色的权限
        PermissionDTO[] GetByRoleId(long roleId);

        //给角色roleId增加权限项id permIds
        void AddPermIds(long roleId, long[] permIds);

        //更新角色role的权限项：先删除再添加
        void UpdatePermIds(long roleId, long[] permIds);
    }
}
