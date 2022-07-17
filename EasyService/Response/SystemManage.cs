using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.Response;

#region 用户管理

public class AdminDto : BaseDto
{
    public long id { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    public string account { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    public string email { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? memo { get; set; }
}

public class RoleAdminDto
{
    /// <summary>
    /// 已分配用户
    /// </summary>
    public List<AdminDto> HasAssign { get; set; }
    /// <summary>
    /// 其他用户
    /// </summary>
    public List<AdminDto> Others { get; set; }
}
#endregion

#region 菜单管理

public class MenuDto
{
    public long id { get; set; }
    public long parentId { get; set; }
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 菜单编码
    /// </summary>
    public string symbol { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    public string? icon { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int sort { get; set; }
}
public class FuncDto
{
    public long id { get; init; }
    /// <summary>
    /// 菜单Id
    /// </summary>
    public long menuId { get; private set; }
    /// <summary>
    /// 功能名称
    /// </summary>
    public string name { get; private set; }
    /// <summary>
    /// 编码
    /// </summary>
    public string symbol { get; private set; }
    /// <summary>
    /// 说明
    /// </summary>
    public string description { get; private set; }
}

/// <summary>
/// 菜单+功能按钮
/// </summary>
public class MenuRsp
{
    /// <summary>
    /// 菜单集合
    /// </summary>
    public List<MenuDto> menus { get; set; }
    /// <summary>
    /// 按钮集合
    /// </summary>
    public List<FuncDto> funcs { get; set; }
}

#endregion

#region 角色管理

public class RoleDto : BaseDto
{
    public long id { get; set; }
    /// <summary>
    /// 角色名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string memo { get; set; }
}

/// <summary>
/// 角色权限
/// </summary>
public class RoleAuthDto
{
    /// <summary>
    /// 菜单集合
    /// </summary>
    public List<MenuDto> menus { get; set; }
    /// <summary>
    /// 功能集合
    /// </summary>
    public List<RoleFunc> funcs { get; set; }
    /// <summary>
    /// 功能
    /// </summary>
    public class RoleFunc : FuncDto
    {
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasAuth { get; set; }
    }
}
#endregion