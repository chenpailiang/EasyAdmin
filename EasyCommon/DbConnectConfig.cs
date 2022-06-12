using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public class DbConnectConfig
{
    public static readonly DbConnectConfig config = new();

    public string adminMaster { get; init; }
    public string goodsMaster { get; init; }
}
