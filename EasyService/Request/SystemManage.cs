namespace EasyService.Request;

#region 菜单/功能管理

public class AddMenuReq
{
    public int parentId { get; set; }
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 编码
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

public class EditMenuReq : AddMenuReq
{
    public int id { get; set; }
}

public class AddFuncReq
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    public int menuId { get; set; }
    /// <summary>
    /// 功能名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 编码
    /// </summary>
    public string symbol { get; set; }
    /// <summary>
    /// 权限值
    /// </summary>
    public int authId { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    public string description { get; set; }
}

public class EditFuncReq
{
    public int id { get; set; }
    /// <summary>
    /// 功能名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 编码
    /// </summary>
    public string symbol { get; set; }
    /// <summary>
    /// 权限值
    /// </summary>
    public int authId { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    public string description { get; set; }
}

#endregion

#region 用户管理

/// <summary>
/// 新增管理员
/// </summary>
public class AddAdminReq
{
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

/// <summary>
/// 编辑管理员
/// </summary>
public class EditAdminReq
{
    public int id { get; set; }
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
#endregion

#region 角色管理

public class AddRoleReq
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string memo { get; set; }
}

public class EditRoleReq : AddRoleReq
{
    public int id { get; set; }
}
#endregion


