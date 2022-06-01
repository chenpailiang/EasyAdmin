﻿using EasyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

public class MenuFunc : AdminEntity
{
    public int id { get; init; }
    public int menuId { get; private set; }
    public string symbol { get; private set; }
    public int authId { get; private set; }
    public bool oust { get; private set; }
}