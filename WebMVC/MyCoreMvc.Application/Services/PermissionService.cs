using MyApplication.Permissions;
using MyCoreMvc.Entitys;
using MyCoreMvc.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMvc.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> _permissionRepository;
        public PermissionService(IRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public Permission Add(Permission permission)
        {
            return _permissionRepository.Insert(permission);
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Delete(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Permission Edit(Permission permission)
        {
            return _permissionRepository.Update(permission);
        }

        public Permission Get(int id)
        {
            return _permissionRepository.Get(id);
        }

        public IQueryable<Permission> GetAll()
        {
            return _permissionRepository.GetAll();
        }
    }
}
