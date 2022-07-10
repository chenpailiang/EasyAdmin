using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public static class Utility
{
    public const int SuperAdmin = -1;
    public static bool Nil(this string? str) => string.IsNullOrWhiteSpace(str);
    public static bool Nil(this IEnumerable list) => list == null || !list.GetEnumerator().MoveNext();

    public static BadRequestException BadRequest(string msg) => new(msg);
    public static NotFoundException NotFound(string msg) => new(msg);

    public static DateTime Now() => DateTime.Now;
    public static int[] EnumToNumberList(Type enumType) => (int[])Enum.GetValues(enumType);
    public static Dictionary<int, string> EnumToDictionary(Type enumType)
    {
        Dictionary<int, string> dict = new();
        foreach (var item in Enum.GetValues(enumType))
        {
            dict.Add(Convert.ToInt32(item), item.ToString());
        }
        return dict;
    }
}
