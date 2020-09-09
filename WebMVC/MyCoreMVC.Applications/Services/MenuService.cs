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

    public class MenuService: IMenuService
    {
        private readonly IRepository<Menu,int> _menuRepository;
        public MenuService(IRepository<Menu, int> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<MenuDto> GetAll(MenuInputDto dto)
        {
            var query = _menuRepository.GetAll();
            if (string.IsNullOrEmpty(dto?.SearchMenuName))
            {
                query = query.Where(u => u.MenuName.Contains(dto.SearchMenuName));
            }
            return AutoMapperExtension.MapTo<Menu, MenuDto>(query).AsQueryable();
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public IQueryable<MenuDto> GetAll()
        {
            var query = _menuRepository.GetAll();
            return AutoMapperExtension.MapTo<Menu, MenuDto>(query).AsQueryable();
        }

        /// <summary>
        /// 分页获取权限列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<MenuDto> GetPageList(MenuInputDto dto)
        {
            var query = _menuRepository.GetAll();
            int count = query.Count();
            if (string.IsNullOrEmpty(dto?.SearchMenuName))
            {
                query = query.Where(u => u.MenuName.Contains(dto.SearchMenuName));
            }
            query = query.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize);
            return AutoMapperExtension.MapTo<Menu, MenuDto>(query).AsQueryable();
        }

        /// <summary>
        /// 根据id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuDto Get(long id)
        {
            var role = _menuRepository.Get((int)id);
            if (role != null)
            {
                return AutoMapperExtension.MapTo<Menu, MenuDto>(role);
            }
            return null;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MenuDto Add(MenuDto dto)
        {
            var query = _menuRepository.GetAll().Where(r => r.MenuName.Contains(dto.MenuName)).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("添加失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<MenuDto, Menu>(dto);
            var result = _menuRepository.Insert(role);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<Menu, MenuDto>(result);
            }
            return null;
        }
        /// <summary>
        /// 根据id删除权限
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            _menuRepository.Delete((int)id);
        }


        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public MenuDto Update(MenuDto dto)
        {
            var query = _menuRepository.GetAll().Where(r => r.MenuName.Contains(dto.MenuName)&&r.Id!=dto.Id).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("修改失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<MenuDto, Menu>(dto);
            var result = _menuRepository.Update(role);
            if (result != null)
            {
                return AutoMapperExtension.MapTo<Menu, MenuDto>(result);
            }
            return null;
        }
    }
}
