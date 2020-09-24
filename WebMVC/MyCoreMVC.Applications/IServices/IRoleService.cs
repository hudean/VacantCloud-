using System.Linq;
using System.Threading.Tasks;
using VaCant.Applications.Dtos;
using VaCant.Entitys;

namespace VaCant.Applications.IServices
{
    /// <summary>
    /// 角色业务
    /// </summary>
    public interface IRoleService : IBaseService
    {
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<RoleDto> GetAll(RoleInputDto dto);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public IQueryable<Role> GetAll();

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<RoleDto> GetPageList(RoleInputDto dto);

        /// <summary>
        /// 根据id获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleDto Get(long id);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<RoleDto> AddAsync(RoleDto dto);

        /// <summary>
        /// 根据id删除角色
        /// </summary>
        /// <param name="id"></param>
        public Task DeleteAsync(long id);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<RoleDto> UpdateAsync(RoleDto dto);
    }
}