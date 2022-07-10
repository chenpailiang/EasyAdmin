global using EasyCommon;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
             db.Aop.OnError = exp =>
             {
                 NLog.LogManager.GetLogger("SQLErrorLogger").Error("SQL:" + exp.Sql);
             };
             db.Aop.OnLogExecuting = (sql, pars) =>
             {
                 Console.WriteLine("SQL:" + sql + "; Parameter: " + string.Join(",", pars.Select(it => it.ParameterName + ":" + it.Value)));
             };
             db.Aop.OnDiffLogEvent = it =>
             {
                 //var tableName = it.BusinessData.ToString();
                 //var log = new
                 //{
                 //    tableName,
                 //    changeType = it.DiffType.ToString(),
                 //    sql = it.Sql,
                 //    parameter = it.Parameters.Select(x => new { name = x.ParameterName, value = x.Value }),
                 //};
                 //NLog.LogManager.GetLogger($"SqlChangeLogger").Info(JsonConvert.SerializeObject(log));
             };
         });

}

public static class DbExtension
{
    public static SqlSugarProvider Getdb(this BaseEntity entity)
    {
        return DbContext.Db.GetConnection(DbId.Admin);
    }

    public static int ExecuteInsert<T>(this IInsertable<T> insertable) where T : BaseEntity, new()
    {
        var tableName = insertable.InsertBuilder.GetTableNameString;
        return insertable.EnableDiffLogEvent().ExecuteCommand();

    }
    public static int ExecuteUpdate<T>(this IUpdateable<T> updateable, CurrentUser currentUser) where T : BaseEntity, new()
    {
        var tableName = updateable.UpdateBuilder.TableName;
        if (updateable.UpdateParameterIsNull) // 表达式更新
            return updateable.SetColumns(x => x.updator == currentUser.account).IgnoreColumns(x => x.oust).EnableDiffLogEvent(tableName).ExecuteCommand();
        else  // 实体更新
            return updateable.ReSetValue(x => x.updator = currentUser.account).IgnoreColumns(x => x.oust).EnableDiffLogEvent(tableName).ExecuteCommand();
    }

    public static int ExecuteDelete<T>(this IUpdateable<T> updateable, CurrentUser currentUser) where T : BaseEntity, new()
    {
        var tableName = updateable.UpdateBuilder.TableName;
        if (updateable.UpdateParameterIsNull) // 表达式更新
            return updateable.SetColumns(x => new T { oust = x.id, updator = currentUser.account }).EnableDiffLogEvent(tableName).ExecuteCommand();
        else // 实体更新
            return updateable.UpdateColumns(x => new { x.oust, x.updator }).ReSetValue(x => x.oust = x.id).ReSetValue(x => x.updator = currentUser.account)
               .EnableDiffLogEvent(tableName).ExecuteCommand();
    }
}


public abstract class BaseEntity
{
    public long id { get; init; }

    [SugarColumn(IsOnlyIgnoreUpdate = true)]
    public string creator { get; protected set; }

    [SugarColumn(IsOnlyIgnoreUpdate = true, IsOnlyIgnoreInsert = true)]
    public DateTime createTime { get; init; }
    public string updator { get; set; }

    [SugarColumn(IsOnlyIgnoreUpdate = true, IsOnlyIgnoreInsert = true)]
    public DateTime updateTime { get; init; }

    [SugarColumn(IsOnlyIgnoreInsert = true)]
    public long oust { get; set; }

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