﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public static class Utility
{
    public const int SuperAdmin = 1;

    public static bool Nil(this string? str) => string.IsNullOrWhiteSpace(str);

    public static BadRequestException BadRequestExp(string msg) => new(msg);

    public static DateTime Now() => DateTime.Now;
}
