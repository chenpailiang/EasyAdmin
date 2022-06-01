namespace EasyService.Request;

#region 菜单管理

public class AddMenuReq
{
    public int parentId { get; set; }
    public string name { get; set; }
    public string? path { get; set; }
    public string? component { get; set; }
    public string? redirect { get; set; }
    public string title { get; set; }
    public string? icon { get; set; }
}

#endregion

#region 用户管理

/// <summary>
/// 新增账户
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

#endregion


