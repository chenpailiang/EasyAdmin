﻿using EasyCommon;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

public class Role : AdminEntity
{
    public int id { get; init; }
    public string name { get; private set; }
    public string memo { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<int> menus { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<int> funcs { get; private set; }
}
