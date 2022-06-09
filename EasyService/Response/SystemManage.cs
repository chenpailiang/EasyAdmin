using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.Response;

public class AdminDto : BaseDto
{
    public int id { get; set; }
    public string account { get; set; }
    public string name { get; set; }
    public string memo { get; set; }
    public List<string> roles { get; set; }
}


#region 菜单管理

public class MenuDto : BaseDto
{
    public int id { get; set; }
    public int parentId { get; set; }
    public string name { get; set; }
    public string title { get; set; }
    public string? icon { get; set; }
    public int sort { get; set; }
}
public class FuncDto : BaseDto
{
    public int id { get; init; }
    public int menuId { get; private set; }
    public string name { get; private set; }
    public string symbol { get; private set; }
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

public class RoleDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string memo { get; set; }
    public List<int> menus { get; set; }
    public List<int> funcs { get; set; }
}

#endregion