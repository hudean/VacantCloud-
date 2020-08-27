using EntityFrameworkCore;
using EntityFrameworkCore.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApplication.Roles
{
    /// <summary>
    /// 角色业务
    /// </summary>
    public class RoleService: IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public Role Add(Role role)
        {
            return _roleRepository.Insert(role);
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

        public Role Update(Role user)
        {
            throw new NotImplementedException();
        }
    }
}
