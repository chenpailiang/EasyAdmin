namespace EasyService.Request;

#region 菜单/功能管理

public class AddMenuReq
{
    public int parentId { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public string? icon { get; set; }
    public string? path { get; set; }
    public int sort { get; set; }
}

public class EditMenuReq : AddMenuReq
{
    public int id { get; set; }
}

public class AddFuncReq
{
    public int menuId { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public int authId { get; set; }
    public string description { get; set; }
}

public class EditFuncReq
{
    public int id { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public int authId { get; set; }
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


