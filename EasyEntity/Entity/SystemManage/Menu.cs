using EasyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

public class Menu : AdminEntity
{
    public int id { get; init; }
    public int parentId { get; private set; }
    public string name { get; private set; }
    public string title { get; private set; }
    public string icon { get; private set; }
    public string path { get; private set; }
    public string component { get; private set; }
    public string redirect { get; private set; }
    public bool oust { get; private set; }

}
