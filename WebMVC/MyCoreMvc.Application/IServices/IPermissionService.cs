using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMvc.Application.IServices
{
    public interface IPermissionService : IBaseService
    {
        public IQueryable<Permission> GetAll();
        public Permission Get(int id);

        public Permission Add(Permission permission);

        public Permission Edit(Permission permission);

        public int Delete(int id);
        public int Delete(Permission permission);
    }
}
