global using EasyCommon;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity;

public class DbId
{
    public const string Admin = "admin";
    public const string Goods = "goods";
    public const string Order = "order";
}

/// <summary>
/// 
/// </summary>
public class DbContext
{

    private class DbConnectConfig
    {
        public string adminMaster { get; set; }
        public string goodsMaster { get; set; }
    }

    private static DbConnectConfig dbConnectConfig = ConfigTool.Get<DbConnectConfig>("DbConnectConfig");

    private static readonly ConfigureExternalServices configureExternalServices = new ConfigureExternalServices()
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
    };

    public static SqlSugarScope Db = new SqlSugarScope(new List<ConnectionConfig>()
    {
        new ConnectionConfig
        {
            ConfigId = DbId.Admin,
            DbType = SqlSugar.DbType.MySql,
            ConnectionString = dbConnectConfig.adminMaster,
            IsAutoCloseConnection = true,
            ConfigureExternalServices = configureExternalServices
        },
        new ConnectionConfig
        {
            ConfigId = DbId.Goods,
            DbType = SqlSugar.DbType.MySql,
            ConnectionString = dbConnectConfig.goodsMaster,
            IsAutoCloseConnection = true,
            ConfigureExternalServices = configureExternalServices
        },
    },
         db =>
         {
             db.QueryFilter.Add(new SqlFilterItem
             {
                 FilterName = null, //全局假删除过滤
                 FilterValue = it => new SqlFilterResult { Sql = "oust = 0" },
                 IsJoinQuery = false
             });
             //单例参数配置，所有上下文生效
             db.Aop.OnLogExecuting = (sql, pars) =>
             {
                 Console.WriteLine(sql);//输出sql
                 Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
             };
         });

}

public static class DbExtension
{
    public static SqlSugarProvider Getdb(this BaseEntity entity)
    {
        return DbContext.Db.GetConnection(DbId.Admin);
    }
    public static int ExecuteDelete<T>(this LogicDeleteProvider<T> deleteProvider, string updator) where T : BaseEntity, new()
    {
        var db = deleteProvider.Deleteable.Context;
        var where = deleteProvider.DeleteBuilder.GetWhereString[5..];
        var pars = deleteProvider.DeleteBuilder.Parameters;
        var type = typeof(T);
        var tableAttr = type.GetCustomAttribute<SugarTable>();
        var table = tableAttr == null ? type.Name : tableAttr.TableName;
        string sql = string.Format("UPDATE {0} SET {1}={2},{3}='{4}' WHERE {5}", table,
            nameof(BaseEntity.oust), nameof(BaseEntity.id), nameof(BaseEntity.updator), updator, where);
        return db.Ado.ExecuteCommand(sql, pars);
    }
}


public abstract class BaseEntity
{
    public long id { get; init; }

    [SugarColumn(IsOnlyIgnoreUpdate = true)]
    public string creator { get; protected set; }

    [SugarColumn(IsOnlyIgnoreUpdate = true, IsOnlyIgnoreInsert = true)]
    public DateTime createTime { get; init; }
    public string updator { get; protected set; }

    [SugarColumn(IsOnlyIgnoreUpdate = true, IsOnlyIgnoreInsert = true)]
    public DateTime updateTime { get; init; }
    public long oust { get; protected set; }

    [SugarColumn(IsIgnore = true)]
    public SqlSugarProvider db
    {
        get
        {
            TenantAttribute? customAttribute = GetType().GetCustomAttribute<TenantAttribute>();
            object configId = customAttribute == null ? DbId.Admin : customAttribute.configId;
            return DbContext.Db.GetConnection(configId);
        }
    }
}