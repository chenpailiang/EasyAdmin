using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

/// <summary>
/// 
/// </summary>
public class DbContext
{
    public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig
    {
        ConfigId = "admin",
        DbType = SqlSugar.DbType.MySql,
        ConnectionString = "Server=localhost;Uid=root;Database=easyadmin;Pwd=123456;Max Pool Size=75; Min Pool Size=5;",
        IsAutoCloseConnection = true,
    },
         db =>
         {
             //单例参数配置，所有上下文生效
             db.Aop.OnLogExecuting = (sql, pars) =>
             {
                 Console.WriteLine(sql);//输出sql
                 Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
             };
         });

}
public class AdminEntity
{
    public static readonly SqlSugarProvider db = DbContext.Db.GetConnection("admin");
}

public class GoodsEntity
{
    public readonly SqlSugarProvider db = DbContext.Db.GetConnection("goods");
}

public class OrderEntity
{

    public readonly SqlSugarProvider db = DbContext.Db.GetConnection("order");

}