using System.Linq;
using VaCant.Applications.Dtos;

namespace VaCant.Applications.IServices
{
    public interface IPermissionService : IBaseService
    {
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<PermissionDto> GetAll(PermissionInputDto dto);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public IQueryable<PermissionDto> GetAll();

        /// <summary>
        /// 分页获取权限列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<PermissionDto> GetPageList(PermissionInputDto dto);

        /// <summary>
        /// 根据id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PermissionDto Get(long id);

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PermissionDto Add(PermissionDto dto);

        /// <summary>
        /// 根据id删除权限
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public PermissionDto Update(PermissionDto dto);
    }
}