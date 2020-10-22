using System;
using System.Linq;
using System.Threading.Tasks;
using VaCant.Applications.Dtos;
using VaCant.Applications.IServices;
using VaCant.Common;
using VaCant.Entitys;
using VaCant.Repositorys;
using Microsoft.EntityFrameworkCore;

namespace VaCant.Applications.Services
{
    /// <summary>
    /// 角色业务
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RolePermission> _rolePermissionRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public RoleService(IRepository<Role> roleRepository, IRepository<User> userRepository, IRepository<RolePermission> rolePermissionRepository, IRepository<UserRole> userRoleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userRoleRepository = userRoleRepository;
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
        public IQueryable<Role> GetAll()
        {
            var query = _roleRepository.GetAll();
            return query;
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
        public async Task<RoleDto> AddAsync(RoleDto inputDto)
        {
            var query = _roleRepository.GetAll().Where(r => r.RoleName.Contains(inputDto.RoleName)).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("添加失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<RoleDto, Role>(inputDto);

            var result = _roleRepository.Insert(role);
            var permissionIds = inputDto.PermissionIds;

            if (result != null)
            {
                permissionIds.ForEach(r =>
                {
                    _rolePermissionRepository.InsertAsync(new RolePermission()
                    {
                        PermissionId = r,
                        RoleId = inputDto.Id
                    });
                });
                return inputDto;
            }
            return null;
        }

        /// <summary>
        /// 根据id删除角色
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(long id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<RoleDto> UpdateAsync(RoleDto inputDto)
        {
            var query = _roleRepository.GetAll().Where(r => r.RoleName.Contains(inputDto.RoleName) && r.Id != inputDto.Id).FirstOrDefault();
            if (query != null)
            {
                throw new AggregateException("修改失败，角色名称已存在！");
            }
            var role = AutoMapperExtension.MapTo<RoleDto, Role>(inputDto);
            var result = _roleRepository.Update(role);
            var permissionIds = inputDto.PermissionIds;
            var rolePermissions = await _rolePermissionRepository.GetAllListAsync(t => t.RoleId == inputDto.Id);
            rolePermissions.ForEach(async r =>
            {
                if (!permissionIds.Contains(r.PermissionId))
                {
                    await _rolePermissionRepository.DeleteAsync(r);
                }
                else
                {
                    permissionIds.Remove(r.PermissionId);
                }
            });
            permissionIds.ForEach(r =>
            {
                _rolePermissionRepository.InsertAsync(new RolePermission()
                {
                    PermissionId = r,
                    RoleId = inputDto.Id
                });
            });
            if (result != null)
            {
                return inputDto;
            }
            return null;
        }

        /// <summary>
        /// 根据登入的用户获取对应角色下的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IQueryable<RolePermission> GetPermissionByUser(long userId)
        {
           var roleIds= _userRoleRepository.GetAll().Where(r => r.UserId == userId).ToList().Select(r=>r.RoleId);
           return _rolePermissionRepository.GetAll().Where(r => roleIds.Contains(r.RoleId)).Include(r => r.Permission);
        }
    }
}