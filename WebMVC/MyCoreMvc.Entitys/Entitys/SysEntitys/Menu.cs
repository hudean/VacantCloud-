﻿using System.ComponentModel.DataAnnotations.Schema;

namespace VaCant.Entitys
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("Menus")]
    public class Menu : BaseEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>

        public int ParentId { get; set; }
        /// <summary>
        /// 路由
        /// </summary>

        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 类型,菜单=0,页面=1
        /// </summary>
        public ActionType Type { get; set; }

        /// <summary>
        /// 是否需要权限(仅页面有效)
        /// </summary>
        public bool NeedAction { get; set; }
    }

    /// <summary>
    /// 类型,菜单=0,页面=1
    /// </summary>
    public enum ActionType
    {
        菜单 = 0,
        页面 = 1
    }
}