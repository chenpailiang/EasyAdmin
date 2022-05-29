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

public class MenuDto
{
    public int id { get; set; }
    public int parentId { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    public List<MenuFunc> funcs { get; set; }
    public class MenuFunc
    {
        public int id { get; set; }
        public string elId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool oust { get; set; }
    }
}

public class Role
{
    public int id { get; set; }
    public string name { get; set; }
    public string memo { get; set; }
    public List<int> menus { get; set; }
    public List<int> funcs { get; set; }
}
