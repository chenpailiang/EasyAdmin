using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAuthCenter
{
    public class Admin
    {
        public int id { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string pwd { get; set; }
        public string memo { get; set; }

        [SqlSugar.SugarColumn(IsJson = true)]
        public List<int> roles { get; set; }
        public bool oust { get; set; }
    }

    public class Menu
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public bool oust { get; set; }
    }

    public class MenuFunc
    {
        public int id { get; set; }
        public int menuId { get; set; }
        public string elId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool oust { get; set; }
    }

    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public string memo { get; set; }
        public List<int> menus { get; set; }
        public List<int> funcs { get; set; }
    }
}
