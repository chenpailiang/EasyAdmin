using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;


/// <summary>
/// 所有权限Id
/// </summary>
public enum AuthEnum : uint
{
    // 用户管理
    at100 = 100,
    at101 = 101,
    at102 = 102,

    // 角色管理
    at120 = 120,
    at121 = 121,
    at122 = 122,

    // 菜单管理
    at140 = 140,
    at141 = 141,
    at142 = 142,

    // 超级权限
    sup = 999
}