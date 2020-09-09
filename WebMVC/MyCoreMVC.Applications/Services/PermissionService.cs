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
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> _permissionRepository;
        public PermissionService(IRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<PermissionDto> GetAll(PermissionInputDto dto)
        {
            var query = _permissionRepository.GetAll();
            if (string.IsNullOrEmpty(dto?.SearchPermissionName))
            {
                query = query.Where(u => u.PermissionName.Contains(dto.SearchPermissionName));
            }
            return AutoMapperExtension.MapTo<Permission, PermissionDto>(query).AsQueryable();
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public IQueryable<PermissionDto> GetAll()
        {
            var query = _permissionRepository.GetAll();
            return AutoMapperExtension.MapTo<Permission, PermissionDto>(query).AsQueryable();
        }

        /// <summary>
        /// 分页获取权限列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<PermissionDto> GetPageList(PermissionInputDto dto)
        {
            var query = _permissionRepository.GetAll();
            int count = query.Count();
            if (string.IsNullOrEmpty(dto?.SearchPermissionName))
            {
                query = query.Where(u => u.PermissionName.Contains(dto.SearchPermissionName));
            }
            query = query.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize);
            return AutoMapperExtension.MapTo<Permission, PermissionDto>(query).AsQueryable();
        }

        /// <summary>
        /// 根据id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PermissionDto Get(long id)
        {
            var role = _permissionRepository.Get(id);
            if (role != null)
            {
                return AutoMapperExtension.MapTo<Permission, PermissionDto>(role);
            }
            return null;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PermissionDto Add(PermissionDto dto)
        {
            var query = _permissionRepository.GetAll().Where(r => r.PermissionName.Contains(dto.PermissionName)).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("添加失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<PermissionDto, Permission>(dto);
            var result = _permissionRepository.Insert(role);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<Permission, PermissionDto>(result);
            }
            return null;
        }
        /// <summary>
        /// 根据id删除权限
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            _permissionRepository.Delete(id);
        }


        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public PermissionDto Update(PermissionDto dto)
        {
            var query = _permissionRepository.GetAll().Where(r => r.PermissionName.Contains(dto.PermissionName)&&r.Id!=dto.Id).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("修改失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<PermissionDto, Permission>(dto);
            var result = _permissionRepository.Update(role);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<Permission, PermissionDto>(result);
            }
            return null;
        }
    }
}
