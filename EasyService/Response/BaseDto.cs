using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.Response;

public abstract class BaseDto
{
    public string creator { get; set; }
    public DateTime createTime { get; set; }
    public string updator { get; set; }
    public DateTime updateTime { get; set; }
}
