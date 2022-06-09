using SqlSugar;
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

        [SugarColumn(IsJson = true)]
        public List<int> roles { get; set; }
        public int oust { get; set; }
    }

    public class Menu
    {
        public int id { get; init; }
        public int oust { get; private set; }
    }

    public class MenuFunc
    {
        public int id { get; init; }
        public int menuId { get; private set; }
        public int authId { get; private set; }
        public int oust { get; private set; }
    }

    public class Role
    {
        public int id { get; set; }

        [SugarColumn(IsJson = true)]
        public List<int> menus { get; private set; }

        [SugarColumn(IsJson = true)]
        public List<int> funcs { get; private set; }
        public int oust { get; private set; }
    }
}
