namespace EasyService.Request;

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
