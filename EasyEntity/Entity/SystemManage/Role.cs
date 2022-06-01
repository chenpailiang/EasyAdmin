using EasyCommon;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

public class Role : AdminEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public RoleEnum role { get; set; }

    [SugarColumn(IsIgnore = true)]
    public string name { get => role.ToString(); }
    public string memo { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<int> menus { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<int> funcs { get; private set; }
}
