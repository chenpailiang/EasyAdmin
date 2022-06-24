global using EasyCommon;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity;

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
            ConfigId = "admin",
            DbType = SqlSugar.DbType.MySql,
            ConnectionString = dbConnectConfig.adminMaster,
            IsAutoCloseConnection = true,
            ConfigureExternalServices = configureExternalServices
        },
        new ConnectionConfig
        {
            ConfigId = "goods",
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

    public static int ExecuteCommand<T>(this LogicDeleteProvider<T> deleteProvider, T deleteObject) where T : BaseEntity, new()
    {
        var db = deleteProvider.Deleteable.Context;
        var where = deleteProvider.DeleteBuilder.GetWhereString[5..];
        var pars = deleteProvider.DeleteBuilder.Parameters;

        IUpdateable<T> updateable = db.Updateable<T>().SetColumns(nameof(BaseEntity.oust), deleteObject.id);
        if (deleteObject.updator != null)
            updateable.SetColumns(nameof(deleteObject.updator), deleteObject.updator);
        if (pars != null)
            updateable.UpdateBuilder.Parameters.AddRange(pars);
        return updateable.Where(where).ExecuteCommand();
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
    public int oust { get; protected set; }
}

public abstract class AdminEntity : BaseEntity
{
    public static readonly SqlSugarProvider db = DbContext.Db.GetConnection("admin");
}

public class GoodsEntity : BaseEntity
{
    public readonly SqlSugarProvider db = DbContext.Db.GetConnection("goods");
}

public class OrderEntity : BaseEntity
{

    public readonly SqlSugarProvider db = DbContext.Db.GetConnection("order");

}
