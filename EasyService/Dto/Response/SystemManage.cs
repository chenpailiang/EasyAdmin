using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.Response;

public class AdminDto
{
    public int id { get; set; }
    public string account { get; set; }
    public string name { get; set; }
    public string memo { get; set; }
    public List<string> roles { get; set; }
}

/// <summary>
/// 用户有权限的菜单
/// </summary>
public class RoleMenuRsp
{
    /// <summary>
    /// 菜单集合
    /// </summary>
    public List<MenuDto> menus { get; set; }
    /// <summary>
    /// 按钮集合
    /// </summary>
    public List<FuncDto> funcs { get; set; }
    public class MenuDto
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string component { get; set; }
        public string redirect { get; set; }
        public Meta meta { get; set; }
        public class Meta
        {
            public string title { get; set; }
            public string icon { get; set; }
        }
    }
    public class FuncDto
    {
        public int menuId { get; set; }
        public string symbol { get; set; }
    }
}

public class RoleDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string memo { get; set; }
    public List<int> menus { get; set; }
    public List<int> funcs { get; set; }
}
