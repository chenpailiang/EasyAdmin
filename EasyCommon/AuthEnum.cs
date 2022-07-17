using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

/// <summary>
/// 所有权限Id
/// </summary>
public enum AuthEnum : uint
{

    /// <summary>
    /// 用户管理-查询
    /// </summary>
    at100 = 100,
    /// <summary>
    /// 用户管理-新增
    /// </summary>
    at101 = 101,
    /// <summary>
    /// 用户管理-编辑
    /// </summary>
    at102 = 102,
    /// <summary>
    /// 用户管理-删除
    /// </summary>
    at103 = 103,
    /// <summary>
    /// 根据角色获取用户
    /// </summary>
    at104 = 104,

    /// <summary>
    /// 角色管理-查询
    /// </summary>
    at110 = 110,
    /// <summary>
    /// 角色管理-新增
    /// </summary>
    at111 = 111,
    /// <summary>
    /// 角色管理-编辑
    /// </summary>
    at112 = 112,
    /// <summary>
    /// 角色管理-删除
    /// </summary>
    at113 = 113,
    /// <summary>
    /// 角色管理-给角色分配权限
    /// </summary>
    at114 = 114,
    /// <summary>
    /// 角色管理-给用户分配角色
    /// </summary>
    at115 = 115,

    /// <summary>
    /// 菜单管理-查询
    /// </summary>
    at120 = 120,
    /// <summary>
    /// 菜单管理-新增
    /// </summary>
    at121 = 121,
    /// <summary>
    /// 菜单管理-编辑
    /// </summary>
    at122 = 122,
    /// <summary>
    /// 菜单管理-删除
    /// </summary>
    at123 = 123,

    /// <summary>
    /// 菜单管理-新增功能
    /// </summary>
    at131 = 131,
    /// <summary>
    /// 菜单管理-编辑功能
    /// </summary>
    at132 = 132,
    /// <summary>
    /// 菜单管理-删除功能
    /// </summary>
    at133 = 133,
}