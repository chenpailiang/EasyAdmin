using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAuthCenter.Utils;

public class DbContext
{
    public static SqlSugarScope Db { get; private set; }
    public static void Init(string dbConnectString)
    {
        Db = new SqlSugarScope(new ConnectionConfig
        {
            DbType = SqlSugar.DbType.MySql,
            ConnectionString = dbConnectString,
            IsAutoCloseConnection = true,
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityService = (t, column) =>
                {
                    if (column.PropertyName.ToLower() == "id") //是id的设为主键
                    {
                        column.IsPrimarykey = true;
                        if (column.PropertyInfo.PropertyType == typeof(int) || column.PropertyInfo.PropertyType == typeof(long)) //是id并且是int/long的是自增
                        {
                            column.IsIdentity = true;
                        }
                    }
                }
            }
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
}
