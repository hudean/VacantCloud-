using System;

namespace VaCant.Applications.Dtos
{
    [Serializable]
    public abstract class BaseDto : BaseDto<long>
    {
    }

    [Serializable]
    public abstract class BaseDto<T>
    {
        public T Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }

    public abstract class BaseInputDto
    {
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsPagination { get; set; } = true;

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}