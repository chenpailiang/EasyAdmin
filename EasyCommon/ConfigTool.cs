using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public static class ConfigTool
{
    public static IConfiguration config;
    public static T Get<T>(string key) where T : class => config.GetSection(key).Get<T>();
}
